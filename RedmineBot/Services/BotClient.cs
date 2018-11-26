using System;
using System.Net;
using System.Net.Http;
using System.Security.Authentication;
using Microsoft.Extensions.Options;
using RedmineBot.Helpers;
using Telegram.Bot;

namespace RedmineBot.Services
{
    public class BotClient : IBotClient
    {
        public TelegramBotClient Client { get; }

        public BotClient(IOptions<BotConfiguration> config)
        {
            Client = SetClient(config);
        }

        private TelegramBotClient SetClient(IOptions<BotConfiguration> config)
        {
            if (!string.IsNullOrEmpty(config.Value.Socks5Host) && config.Value.Socks5Port != default)
            {
                if (!string.IsNullOrEmpty(config.Value.UserName) || !string.IsNullOrEmpty(config.Value.Password))
                    return new TelegramBotClient(config.Value.BotToken, 
                        GetProxyClient(config.Value.Socks5Host, config.Value.Socks5Port, config.Value.UserName, config.Value.Password));

                return new TelegramBotClient(config.Value.BotToken, GetProxyClient(config.Value.Socks5Host, config.Value.Socks5Port));
            }

            if (string.IsNullOrEmpty(config.Value.BotToken))
                throw new ArgumentNullException(nameof(BotConfiguration.BotToken),"Bot token can't be empty");

            return new TelegramBotClient(config.Value.BotToken);
        }

        private static HttpClient GetProxyClient(string host, int port, string username = default, string password = default)
        {
            var proxy = username != default && password != default
                ? new WebProxy($"{host}:{port}/", false, new string[] { }, new NetworkCredential(username, password))
                : new WebProxy($"{host}:{port}/", false, new string[] { });

            var httpClientHandler = new HttpClientHandler()
            {
                Proxy = proxy,
                SslProtocols = SslProtocols.Tls
            };

            var client = new HttpClient(handler: httpClientHandler, disposeHandler: true);
            return client;
        }
    }
}
