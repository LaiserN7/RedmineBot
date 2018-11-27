using System.Threading.Tasks;

namespace RedmineBot.Services
{
    public interface IBotService
    {
        Task GetHelp(long chatId);
        Task SendText(long chatId, string message);
        Task GetMenu(long chatId);
    }
}
