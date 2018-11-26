using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace RedmineBot.Helpers
{
    public class Request
    {
        public const string DefaultMessagePath = "api/update";

        public static async Task<string> RequestBody(HttpContext context)
        {
            string body;
            long startingPosition = context.Request.Body.Position;
            context.Request.Body.Seek(0, SeekOrigin.Begin);
            using (var reader = new StreamReader(context.Request.Body))
            {
                body = await reader.ReadToEndAsync();
            }
            context.Request.Body.Seek(
                startingPosition < context.Request.Body.Length ? startingPosition : context.Request.Body.Length,
                SeekOrigin.Begin);
            return body;
        }

        public static async Task<(long chatId, string message)> GetInfo(HttpContext context)
        {
            try
            {
                if (!context.Request.Path.Value.ToLower().Contains(DefaultMessagePath))
                    return (default, string.Empty);

                var body = await RequestBody(context);
                var update = JsonConvert.DeserializeObject<Update>(body);

                if (update.Type != UpdateType.Message)
                    return (default, string.Empty);

                var chatId = update.Message.Chat.Id;
                var message = update.Message.Text;

                return (chatId, message);
            }
            catch (Exception exception)
            {
                return (0, exception.Message);
            }
        }
    }
}
