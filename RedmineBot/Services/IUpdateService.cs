using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace RedmineBot.Services
{
    public interface IUpdateService
    {
        Task EchoAsync(Update update);
    }
}