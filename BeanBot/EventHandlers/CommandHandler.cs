using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System;
using BeanBot.Services.AWS;
using BeanBot.Models;
using Serilog;
using System.Reflection;

namespace BeanBot.EventHandlers
{
    public class CommandHandler
    {
        private readonly DiscordShardedClient _discordClient;
        private readonly CommandService _commandService;
        private readonly IConfiguration _config;
        private readonly IServiceProvider _serviceProvider;

        // DiscordSocketClient, CommandService, IConfigurationRoot, and IServiceProvider are injected automatically from the IServiceProvider
        public CommandHandler(DiscordShardedClient discordClient, CommandService commandService, IConfiguration config, IServiceProvider serviceProvider)
        {
            _discordClient = discordClient;
            _commandService = commandService;
            _config = config;
            _serviceProvider = serviceProvider;
        }

        public async Task InitializeCommandsAsync()
        {
            Log.Information("Installing Commands");
            _discordClient.MessageReceived += HandleCommandsAsync;
            await _commandService.AddModulesAsync(Assembly.GetEntryAssembly(), _serviceProvider);
        }

        private async Task HandleCommandsAsync(SocketMessage messageEvent)
        {
            var discordMessage = messageEvent as SocketUserMessage;
            if (MessageIsSystemMessage(discordMessage))
                return; //Return and ignore if the message is a discord system message
            int argPos = 0;
            if (!MessageHasCommandPrefix(discordMessage, ref argPos) ||
                messageEvent.Author.IsBot)
                return; //Return and ignore if the discord message does not have the command prefixes or if the author of the message is a bot
            var context = new ShardedCommandContext(_discordClient, discordMessage);
            await _commandService.ExecuteAsync(
                context: context,
                argPos: argPos,
                services: _serviceProvider);
        }

        private bool MessageHasCommandPrefix(SocketUserMessage discordMessage, ref int argPos)
        {
            return (discordMessage.HasStringPrefix("succ ", ref argPos, StringComparison.OrdinalIgnoreCase) ||
                            discordMessage.HasMentionPrefix(_discordClient.CurrentUser, ref argPos) ||
                            discordMessage.HasCharPrefix('%', ref argPos));
        }

        private bool MessageIsSystemMessage(SocketUserMessage discordMessage)
        {
            if (discordMessage == null)
                return true;
            else
                return false;
        }
    }
}
