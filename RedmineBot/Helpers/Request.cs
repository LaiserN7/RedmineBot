using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Newtonsoft.Json;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace RedmineBot.Helpers
{
    public class Request
    {
        public const string DefaultMessagePath = "api/update";

        public static async Task<string> RequestBody(HttpRequest request)
        {
            var body = request.Body;

            

            var buffer = new byte[Convert.ToInt32(request.ContentLength)];

            await request.Body.ReadAsync(buffer, 0, buffer.Length);

            //We convert the byte[] into a string using UTF8 encoding...
            var bodyAsText = Encoding.UTF8.GetString(buffer);

            //..and finally, assign the read body back to the request body, which is allowed because of EnableRewind()
            request.Body = body;

            return bodyAsText;
        }

        public static async Task<(long chatId, string less)> GetInfo(HttpRequest request)
        {
            try
            {
                var body = await RequestBody(request);
                var info = $"{request.Scheme} {request.Host}{request.Path} {request.QueryString} \n {body}";

                if (!request.Path.Value.ToLower().Contains(DefaultMessagePath))
                    return (default, info);

                var update = JsonConvert.DeserializeObject<Update>(body);

                return update.Type != UpdateType.Message 
                    ? (default, info) 
                    : (update.Message.Chat.Id, $"{info} \n\n user text: `{update.Message.Text}` \n");
            }
            catch (Exception exception)
            {
                return (default, exception.Message);
            }
        }
    }
}
