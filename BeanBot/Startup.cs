using Amazon.SQS;
using BeanBot.Data;
using BeanBot.EventHandlers;
using BeanBot.Helpers;
using BeanBot.Models;
using BeanBot.Services;
using BeanBot.Services.AWS;
using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeanBot
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
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddControllersWithViews();
            services.AddSingleton(new DiscordShardedClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Verbose,
                MessageCacheSize = 1000
            }))
            .AddSingleton(new CommandService(new CommandServiceConfig
            {
                LogLevel = LogSeverity.Verbose,
                DefaultRunMode = RunMode.Async,
                CaseSensitiveCommands = false
            }))
            .AddSingleton<InteractiveService>()
            .AddSingleton<LogService>()
            .AddSingleton<StartupService>()
            .AddSingleton<CommandHandler>();
            
            var appSettingsSection = Configuration.GetSection("ServiceConfiguration");
            services.AddAWSService<IAmazonSQS>();
            services.Configure<ServiceConfiguration>(appSettingsSection);
            services.AddTransient<IAWSSQSService, AWSSQSService>();
            services.AddTransient<IAWSSQSHelper, AWSSQSHelper>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });

            var provider = app.ApplicationServices;

            provider.GetRequiredService<LogService>();
            await provider.GetRequiredService<StartupService>().StartAsync();
            await provider.GetRequiredService<CommandHandler>().InitializeCommandsAsync();
        }
    }
}
