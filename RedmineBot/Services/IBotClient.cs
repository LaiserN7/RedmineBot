using Telegram.Bot;

namespace RedmineBot.Services
{
    public interface IBotClient
    {
        TelegramBotClient Client { get; }
    }
}
