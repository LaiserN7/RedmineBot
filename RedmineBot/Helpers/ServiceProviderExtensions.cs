
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RedmineBot.Services;

namespace RedmineBot.Helpers
{
    public static class ServiceProviderExtensions
    {
        public static void RegistrationServices(this IServiceCollection services)
        {
            services.AddSingleton<IBotClient, BotClient>();
            services.AddSingleton<IBotService, BotService>();
            services.AddScoped<IRedmineService, RedmineService>();
            services.AddScoped<IUpdateService, UpdateService>();
        }

        public static void RegistrationConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<BotConfiguration>(configuration.GetSection(nameof(BotConfiguration)));
            services.Configure<RedmineConfiguration>(configuration.GetSection(nameof(RedmineConfiguration)));
            services.Configure<DomainConfiguration>(configuration.GetSection(nameof(DomainConfiguration)));
        }
    }
}
