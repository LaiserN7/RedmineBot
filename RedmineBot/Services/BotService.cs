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
                "`ping` - if bot alive return pong" +
                "`/chatId` - watch id of current chat\n" +
                "`/userId` - watch id of current user\n" +
                "`/rnd` - create random task\n" +
                "`/spend <hours?> <subject?>` - create task with name (if have subject + hours),\n" +
                "spend hours to any ure task (if no, create it)\n";
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
