using BeanBot.Util;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeanBot.Discord
{
    public class DiscordStartup
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly DiscordSocketClient _discordClient;
        private readonly IConfiguration _config;

        public DiscordStartup(IServiceProvider serviceProvider, IConfiguration config, DiscordSocketClient discordClient)
        {
            _serviceProvider = serviceProvider;
            _config = config;
            _discordClient = discordClient;
        }

        public async Task StartAsync()
        {
            var discordToken = _config["DISCORD_BOT_TOKEN"];
            Log.Information($"Discord Bot Token: {discordToken}");

            _discordClient.Log += LogUtil.LogAsync;

            await _discordClient.LoginAsync(TokenType.Bot, discordToken);
            await _discordClient.StartAsync();
            await _discordClient.SetGameAsync("From the ashes I am born anew", null, ActivityType.Playing);
        }
    }
}
