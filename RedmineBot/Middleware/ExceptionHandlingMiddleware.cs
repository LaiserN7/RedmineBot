using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Options;
using RedmineBot.Helpers;
using RedmineBot.Services;
using Telegram.Bot.Types;

namespace RedmineBot.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IBotService _botService;
        private readonly IOptions<BotConfiguration> _config;

        public ExceptionHandlingMiddleware(RequestDelegate next, IBotService botService, IOptions<BotConfiguration> config)
        {
            _next = next;
            _botService = botService;
            _config = config;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            //This line allows us to set the reader for the request back at the beginning of its stream.
            context.Request.EnableRewind();
            (long chatId, string message) = await Request.GetInfo(context.Request);
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception exception)
            {
                await OnException(exception, chatId, message);
            }
        }

        private async Task OnException(Exception exception, long chatId, string message)
        {
            if (chatId != 0 && chatId != _config.Value.DefaultChatId)
                await _botService.SendText(chatId, exception.Message);

            await _botService.SendText(_config.Value.DefaultChatId, $"{message} \n {exception}");
        }
    }
}
