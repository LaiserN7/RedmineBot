using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MihaZupan;
using Redmine.Net.Api;
using Redmine.Net.Api.Async;
using Redmine.Net.Api.Types;
using RedmineBot.Helpers;
using RedmineBot.Services;
using Telegram.Bot;

namespace ConsoleForTest
{
    class Program
    {
        private static readonly HttpToSocks5Proxy Proxy = new HttpToSocks5Proxy("31.13.224.12", 9999);
        private const string BotToken = "631170979:AAFsyz1LJ17GeDGJ6MfSaEl6P0PJKfO81q0";
        // private const string RedmineApiKey = "c52fce8bc7508b51915f3a0b928d3f506ced9a65";//наиль
        private const string RedmineApiKey = "e502c3df9e31fb3a4b5b0b522d609a1bb444fa2e";//рустик
        private const string RedmineHost = "https://rd.d-l-s.ru/";

        //private const string login = "nshakirov";
        //private const string pass = "Z!1eZwVm";

        enum IssueStatus : byte
        {
            New = 1,
            InWork,
            Done,
            OnPause,
            Closed,
            InWorkToday = 7
        }

        static async Task Main(string[] args)
        {
            
            //userId - 65 - Саша
            try
            {
                await Write();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.WriteLine("Lool");

            Console.ReadLine();

        }

        private static async Task Write()
        {
            var manager = new RedmineManager(RedmineHost, RedmineApiKey);

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            var text = "/spend 10";
            (float hours, string subject) = GetTimeAndSubject(text);

            var user = await manager.GetCurrentUserAsync();
            var inWork = new NameValueCollection
            {
                { RedmineKeys.STATUS_ID, $"{IssueStatus.InWork:D}|{IssueStatus.New:D}|{IssueStatus.InWorkToday:D}" },
                { RedmineKeys.DUE_DATE, DateHelpers.GetLastDay().ToString(DateHelpers.DateFormat) },
                { RedmineKeys.ASSIGNED_TO_ID, user.Id.ToString() }
            };
            var myTasks = await manager.GetPaginatedObjectsAsync<Issue>(inWork);
            if (myTasks.TotalCount != 0)
             {
                var filter = new NameValueCollection
                {
                    { RedmineKeys.SPENT_ON, $"><{DateHelpers.GetFirstDay}|{DateHelpers.GetLastDay()}" }
                };
                foreach (var issue in myTasks.Objects)
                {
                    if (issue.EstimatedHours == null) continue;

                    filter[RedmineKeys.ISSUE_ID] = 26510.ToString();//issue.Id.ToString();
                    var timeEntrys = await manager.GetPaginatedObjectsAsync<TimeEntry>(filter);
                    var a = (issue.EstimatedHours);
                    var b = (float) timeEntrys.Objects.Sum(h => h.Hours);
                    var c = hours;
                    var d = a - b - c;
                    var e = d < 0.0f;
                    if (issue.EstimatedHours - (float)timeEntrys.Objects.Sum(h => h.Hours) - hours < 0.0f) continue;

                    return;
                }
                Console.WriteLine("lol");
            }

            hours = hours <= 40.0f ? 40.0f : hours;

            //var newIssue = await _redmineService.Create(Generator.GenerateIssue(user.Id, hours: hours, subject: subject));

            //need to update all status to OnWork????
            //newIssue.Status = new IdentifiableName { Id = (int)IssueStatus.InWork };
            //newIssue.UpdatedOn = DateTime.Now;
            //await _redmineService.Update(newIssue.Id.ToString(), newIssue);

            //await _redmineService.Create(Generator.GenerateTimeEntry(newIssue.Id));

            //await _botService.SendText(_chatId, $"success spend \n {stopWatch.ElapsedMilliseconds} ms");
        }

        private static (float hours, string subject) GetTimeAndSubject(string text)
        {
            if (text.Replace(" ", "") == "/spend") return (8.0f, null);

            float hours = default;
            string subject = default;
            const string pattern = @"^(?<type>/\w+)\s(?<hours>\d+)";
            var m = Regex.Match(text, pattern);
            if (m.Length > 0)
            {
                float.TryParse(m.Groups["hours"].Value, out hours);
                subject = m.Groups["subject"].Value;
            }

            var a =  hours <= 0.0f;
            var b = hours > 168.0f;

            if (hours <= 0.0f || hours > 168.0f)
                throw new ApplicationException("Wrong time format must be between 0 and 168");

            if (string.IsNullOrEmpty(subject))
                subject = null;

            return (hours, subject);

        }

        private static void Bot_OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            if (e.Message.Type == Telegram.Bot.Types.Enums.MessageType.Text)
            {
               
            }
        }

        private static DateTime GetLastDay()
        {
            var today = DateTime.Today;
            var lastDay = DateTime.DaysInMonth(today.Year, today.Month);
            return new DateTime(today.Year, today.Month, lastDay);
        }

        private static Issue GetIssue()
        {
            return new Issue
            {
                AssignedTo= new IdentifiableName { Id= 107 },
                Author = new IdentifiableName { Id= 107 },
                CreatedOn = DateTime.Now,
                CustomFields =
                {
                    new IssueCustomField{Id = 6, Multiple = false/*, Name="Тип"*/, Values = { new CustomFieldValue { Info = "Тех" }}},
                    new IssueCustomField{Id = 13, Multiple = false/*, Name="Deadline"*/, Values = {new CustomFieldValue{Info = DateTime.Today.ToString(CultureInfo.InvariantCulture)}}}
                },
                Description = "",
                DoneRatio = 0,
                Priority = new IdentifiableName { Id= 2 },//normal
                Project = new IdentifiableName { Id = 30 },//tu dji host
                StartDate = DateTime.Today,
                Status = new IdentifiableName { Id = 1 }, //new
                Subject = "Правка временной зоны react",
                Tracker = new IdentifiableName { Id = 5 },
                UpdatedOn = DateTime.Now
            };
        }
    }
}
//example
//107 


// EstimatedHours = 30
//+		AssignedTo	{[IdentifiableName: Id=1, Name=Вадим Кляшев]}	Redmine.Net.Api.Types.IdentifiableName
//+		Author	{[IdentifiableName: Id=1, Name=Вадим Кляшев]}	Redmine.Net.Api.Types.IdentifiableName
//+		CreatedOn	{30.10.2018 9:29:40}	System.DateTime?
//-		CustomFields	Count = 2	System.Collections.Generic.IList<Redmine.Net.Api.Types.IssueCustomField> {System.Collections.Generic.List<Redmine.Net.Api.Types.IssueCustomField>}
//+		[0]	{[IssueCustomField: [IdentifiableName: Id=6, Name=Тип] Values=System.Collections.Generic.List`1[Redmine.Net.Api.Types.CustomFieldValue], Multiple=False]}	Redmine.Net.Api.Types.IssueCustomField
//      Values = Count = 1 [0] = {[CustomFieldValue: Info=Тех]}
//+		[1]	{[IssueCustomField: [IdentifiableName: Id=13, Name=Deadline] Values=System.Collections.Generic.List`1[Redmine.Net.Api.Types.CustomFieldValue], Multiple=False]}	Redmine.Net.Api.Types.IssueCustomField
//                          [0] = {[CustomFieldValue: Info=2018-10-31]}
//		Description	""	string
//		DoneRatio	0	float?
//		IsPrivate	false	bool
//+		Priority	{[IdentifiableName: Id=2, Name=Нормальный]}	Redmine.Net.Api.Types.IdentifiableName
//+		Project	{[IdentifiableName: Id=30, Name=09) DLS Smart Purchase 2G Хост (Вадим Кляшев)]}	Redmine.Net.Api.Types.IdentifiableName
//+		StartDate	{01.10.2018 0:00:00}	System.DateTime?
//+		Status	{[IdentifiableName: Id=2, Name=В работе]}	Redmine.Net.Api.Types.IdentifiableName
//		Subject	"Администрирование серверов и инфраструктуры"	string
//+		Tracker	{[IdentifiableName: Id=5, Name=Задача]}	Redmine.Net.Api.Types.IdentifiableName
//+		UpdatedOn	{30.10.2018 9:29:40}	System.DateTime?


//TimeEntry
//Activity = {[IdentifiableName: Id=9, Name=Разработка]}
//Comments = "оперативка по SwS, совещание с  Леонидом по вопросам ВТБ, обсуждение итогов тестирования ЛК ТСП "
//CreatedOn = {31.10.2018 16:42:51}
//Hours = 1.92
//Issue = {[IdentifiableName: Id=26287, Name=]}
//Project = {[IdentifiableName: Id=546, Name=00) SwS-продукт]}
//SpentOn = {31.10.2018 0:00:00}
//UpdatedOn = {31.10.2018 16:42:51}
//User = {[IdentifiableName: Id=80, Name=Гульнара Фаттахова]}