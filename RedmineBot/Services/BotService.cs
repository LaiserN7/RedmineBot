using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace RedmineBot.Services
{
    public class BotService : IBotService
    {
        private readonly IBotClient _client;

        public BotService(IBotClient client)
        {
            _client = client;
        }

        public Task GetHelp(long chatId)
        {
            const string text =
                "Usage:\n" +
                "`/help` - watch helper info\n" +
                "`ping` - if bot alive return pong\n" +
                "`/chatId` - watch id of current chat\n" +
                "`/userId` - watch id of current user\n" +
                "`/rnd` - create random task\n" +
                "`/spend <hours?>` - spend hours to any ure task default is 8\n" +
                "`/tasks <hours> - show ure opened tasks where ure can spend (spend only in private chat)\n" +
                "`/info - show ure short info\n" +
                "`/close - close all opened tasks\n" +
                "`/create <hours> <subject> - create task";
            //"`/menu` - return menu\n" +
            //"/inline   - send inline keyboard\n" +
            //"/keyboard - send custom keyboard\n" +
            //"/request  - request location or contact\n" + 
            //"/hello - send a hello text\n" + 
            //"/ver - watch a version of bot\n" +
            //"/config - watch a type of config\n" +
            //"/repeat - enable/disable repeat message\n";

            return _client.Client.SendTextMessageAsync(chatId, text);
        }

        public Task SendText(long chatId, string message)
        {
            return _client.Client.SendTextMessageAsync(chatId, message);
        }

        public Task SendTextWithReplyMarkup(long chatId, string title, IReplyMarkup reply )
        {
            return _client.Client.SendTextMessageAsync(chatId, title, replyMarkup: reply);
        }

        public Task GetMenu(long chatId)
        {
            var inlineKeyboard = new InlineKeyboardMarkup(new[]
            {
                new [] // first row
                {
                    InlineKeyboardButton.WithCallbackData("1.1"),
                    InlineKeyboardButton.WithCallbackData("1.2"),
                },
                new [] // second row
                {
                    InlineKeyboardButton.WithCallbackData("2.1"),
                    InlineKeyboardButton.WithCallbackData("2.2"),
                }
            });

            return _client.Client.SendTextMessageAsync(chatId, "Choose", replyMarkup: inlineKeyboard);
        }
    }

   
}
