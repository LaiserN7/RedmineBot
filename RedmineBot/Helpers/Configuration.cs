namespace RedmineBot.Helpers
{
    public class DomainConfiguration
    {
        public Users[] Users { get; set; }
    }

    public class BotConfiguration
    {
        public string BotToken { get; set; }

        public string Socks5Host { get; set; }

        public int Socks5Port { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public long DefaultChatId { get; set; }
    }

    public class RedmineConfiguration
    {
        public string ApiKey { get; set; }

        public string Host { get; set; }
    }

    public class Users
    {
        public string Name { get; set; }

        public long TelegramUserId { get; set; }

        public string RedmineApiKey { get; set; }
    }
}
