using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace BeanBot.Services
{
    public class StartupService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly DiscordShardedClient _discordClient;
        private readonly CommandService _commandService;
        private readonly IConfiguration _config;

        public StartupService(IServiceProvider serviceProvider, DiscordShardedClient discordClient, CommandService commandService, IConfiguration config)
        {
            _serviceProvider = serviceProvider;
            _config = config;
            _discordClient = discordClient;
            _commandService = commandService;
        }

        public async Task StartAsync()
        {
            string discordToken = _config["DISCORD_BOT_TOKEN"];
            if (string.IsNullOrWhiteSpace(discordToken))
                throw new Exception("Please enter your bot's token into the `hostSettings.json` file found in the Config folder");

            await _discordClient.LoginAsync(TokenType.Bot, discordToken);
            await _discordClient.StartAsync();
            await _discordClient.SetGameAsync("My purpose is to bully Hatate", null, ActivityType.Playing);
            _discordClient.ShardReady += (shard) =>
            {
                Log.Information($"Bean Bot Shard Id {shard.ShardId} connected");
                foreach (var server in shard.Guilds)
                {
                    Log.Information($"Guild {server.Name} connected to Bean Bot Shard Id {shard.ShardId}");
                }
                return Task.CompletedTask;
            };

            await _commandService.AddModulesAsync(Assembly.GetEntryAssembly(), _serviceProvider);
        }
    }
}
