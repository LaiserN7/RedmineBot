using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using RedmineBot.Helpers;
using RedmineBot.Services;

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
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception exception)
            {
                await OnException(context, exception);
            }
        }

        private async Task OnException(HttpContext context, Exception exception)
        {
            //(long chatId, string message) = await Request.GetInfo(context);
            var chatId = /*chatId != 0 ? chatId :*/ _config.Value.DefaultChatId;
            var message =/* message + Environment.NewLine +*/ exception.ToString();

            await _botService.SendText(chatId, message);
        }
    }
}
