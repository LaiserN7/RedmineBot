using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace RedmineBot.Services
{
    public interface IBotService
    {
        Task GetHelp(long chatId);
        Task SendText(long chatId, string message);
        Task GetMenu(long chatId);
        Task SendTextWithReplyMarkup(long chatId, string title, IReplyMarkup reply);
    }
}
