using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RedmineBot.Helpers;
using RedmineBot.Middleware;

namespace RedmineBot
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.RegistrationServices();

            services.Configure<BotConfiguration>(Configuration.GetSection(nameof(BotConfiguration)));
            services.Configure<RedmineConfiguration>(Configuration.GetSection(nameof(RedmineConfiguration)));
            services.Configure<DomainConfiguration>(Configuration.GetSection(nameof(DomainConfiguration)));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
                app.UseExceptionMiddleware();

            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
