using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Redmine.Net.Api;
using Redmine.Net.Api.Exceptions;
using Redmine.Net.Api.Types;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using RedmineBot.Helpers;
using IssueStatus = RedmineBot.Helpers.IssueStatus;

namespace RedmineBot.Services
{
    public class UpdateService : IUpdateService
    {
        private const string CommandType = @"^/";
        private readonly IBotService _botService;
        private readonly IRedmineService _redmineService;
        private IOptions<DomainConfiguration> _domainConfig;
        private IOptions<RedmineConfiguration> _redmineConfig;
        private int telegramUserId;
        private long chatId;

        public UpdateService(IBotService botService, IRedmineService redmineService, 
            IOptions<DomainConfiguration> domainConfig, IOptions<RedmineConfiguration> redmineConfig)
        {
            _botService = botService;
            _redmineService = redmineService;
            _domainConfig = domainConfig;
            _redmineConfig = redmineConfig;
        }

        public Task EchoAsync(Update update)
        {
            return Handling(update);
        }

        private Task Handling(Update update)
        {
            switch (update.Type)
            {
                case UpdateType.Message:
                    return HandlingMessage(update.Message);
                case UpdateType.CallbackQuery:
                    return HandlingCallback(update.CallbackQuery);
            }

            return Task.CompletedTask;
        }

        private Task HandlingMessage(Message message)
        {
           
            string text = message.Text;
            chatId = message.Chat.Id;
            //check trusted users?
            telegramUserId = message.From.Id;

            if (Regex.IsMatch(text, CommandType))
            {
                switch (text.Split(string.Empty).First())
                {
                    case "/help":
                        return _botService.GetHelp(chatId);
                    case "/chatId":
                        return _botService.SendText(chatId, $"{chatId}");
                    case "/menu":
                        return _botService.GetMenu(chatId);
                    case "/rnd":
                        return CreateRandomIssue();
                    case "/spend":
                        return SpendTime(text);
                }
            }

            if (Regex.IsMatch(text, @"^ping", RegexOptions.IgnoreCase))
                return _botService.SendText(chatId, "pong");

            return Task.CompletedTask;
        }

        private Task HandlingCallback(CallbackQuery callback)
        {
            throw new NotImplementedException();
        }

        private Task CreateRandomIssue()
        {
            return _redmineService.Create(Generator.GenerateIssue());
        }
       
        private async Task SpendTime(string text)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            (float hours, string subject) = GetTimeAndSubject(text);

            _redmineService.Manager = GetManager(); // set manager for user
            var user = await _redmineService.GetCurrentUser();
            var inWork = new NameValueCollection
            {
                { RedmineKeys.STATUS_ID, $"{IssueStatus.InWork:D}|{IssueStatus.New:D}" },
                { RedmineKeys.DUE_DATE, DateHelpers.GetLastDay().ToString(DateHelpers.DateFormat) },
                { RedmineKeys.ASSIGNED_TO_ID, user.Id.ToString() }
            };
            var myTasks = await _redmineService.GetAll<Issue>(inWork);
            if (myTasks.TotalCount != default)
            {
                var filter = new NameValueCollection
                {
                    { RedmineKeys.SPENT_ON, $"><{DateHelpers.GetFirstDay}|{DateHelpers.GetLastDay()}" }
                };
                foreach (var issue in myTasks.Objects)
                {
                    if (issue.EstimatedHours == null) continue;

                    filter[RedmineKeys.ISSUE_ID] = issue.Id.ToString();
                    var timeEntrys = await _redmineService.GetAll<TimeEntry>(filter);
                    if ( issue.EstimatedHours - (float)timeEntrys.Objects.Sum(h => h.Hours) - hours < 0.0f ) continue;

                    await _redmineService.Create(Generator.GenerateTimeEntry(issue.Id));
                    return;
                }
               
            }

            hours = hours <= 40.0f ? 40.0f : hours; 

            var newIssue = await _redmineService.Create(Generator.GenerateIssue(user.Id, hours: hours, subject: subject));

            //need to update all status to OnWork????
            newIssue.Status = new IdentifiableName { Id = (int) IssueStatus.InWork };
            newIssue.UpdatedOn = DateTime.Now;
            await _redmineService.Update(newIssue.Id.ToString(), newIssue);

            await _redmineService.Create(Generator.GenerateTimeEntry(newIssue.Id));

            await _botService.SendText(chatId, $"success spend {stopWatch.ElapsedMilliseconds} ms");
        }

        private RedmineManager GetManager()
        {
            foreach (var user in _domainConfig.Value.Users)
            {
                if (user.TelegramUserId != telegramUserId) continue;

                return new RedmineManager(_redmineConfig.Value.Host, user.RedmineApiKey);
            }
            
            throw new NotFoundException($"Api key for {nameof(telegramUserId)} = {telegramUserId} not found");
        }

        private (float hours, string subject) GetTimeAndSubject(string text)
        {
            if (text.Replace(" ", "") == "/spend") return (8.0f, null);

            float hours = default;
            string subject = default;
            var pattern = @"^(?<type>/\w+)\s(?<hours>\d+)\s(?<subject>.*)";
            var m = Regex.Match(text, pattern);
            if (m.Length > 0)
            {
                float.TryParse(m.Groups["hours"].Value, out hours);
                subject = m.Groups["subject"].Value;
            }

            if (0.0f <= hours && hours > 168.0f)
                throw new ApplicationException("Wrong time format must be between 0 and 168");

            if (string.IsNullOrEmpty(subject))
                subject = null;

            return (hours, subject);

        }
    }
}
