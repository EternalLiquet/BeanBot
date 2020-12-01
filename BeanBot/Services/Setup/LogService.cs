using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BeanBot.Services
{
    public class LogService
    {
        private readonly string _logDirectory;
        private readonly DiscordShardedClient _client;
        private readonly CommandService _commandService;

        public LogService(DiscordShardedClient client, CommandService commandService)
        {

            _client = client;
            _commandService = commandService;

            _client.Log += LogClientMessages;
            _commandService.CommandExecuted += LogCommands;

            _logDirectory = Program.logDirectory;
        }

        private Task LogClientMessages(LogMessage message)
        {
            if (!Directory.Exists(_logDirectory))
                Directory.CreateDirectory(_logDirectory);

            string formattedMessage = (message.Source != null && message.Message != null) ?
                $"Discord:\t{message.Source}\t{message.Message}" :
                $"Discord:\t{message.ToString()}";
            switch (message.Severity)
            {
                case LogSeverity.Critical:
                    Log.Fatal(formattedMessage);
                    break;
                case LogSeverity.Error:
                    Log.Error(formattedMessage);
                    break;
                case LogSeverity.Warning:
                    Log.Warning(formattedMessage);
                    break;
                case LogSeverity.Info:
                    Log.Information(formattedMessage);
                    break;
                case LogSeverity.Verbose:
                    Log.Verbose(formattedMessage);
                    break;
                default:
                    Log.Information($"Log Severity: {message.Severity}");
                    Log.Information(formattedMessage);
                    break;
            }
            return Task.CompletedTask;
        }

        private Task LogCommands(Optional<CommandInfo> command, ICommandContext context, IResult result)
        {
            var commandName = command.IsSpecified ? command.Value.Name : "Unspecified Command";
            string formattedMessage = $"Discord:\t{commandName} was executed at {DateTime.UtcNow}";
            if (result.IsSuccess)
            {
                Log.Information(formattedMessage);
            }
            else
            {
                Log.Error($"{formattedMessage}\n\t\t\tInput: {context.Message}");
                Log.Error($"{result.Error}, {result.ErrorReason}");
            }
            return Task.CompletedTask;
        }
    }
}
