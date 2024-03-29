﻿using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using SupportBot.Services;

namespace SupportBot
{
    /// <summary>
    /// Class CommandHandler.
    /// </summary>
    public class CommandHandler
    {
        private readonly DatabaseService _databaseService;
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// The client
        /// </summary>
        private readonly DiscordSocketClient _client;
        /// <summary>
        /// The commands
        /// </summary>
        private readonly CommandService _commands;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandHandler"/> class.
        /// </summary>
        /// <param name="services">The services.</param>
        public CommandHandler(DiscordSocketClient client, CommandService commands, DatabaseService databaseService, IServiceProvider serviceProvider)
        {
            _databaseService = databaseService;
            _serviceProvider = serviceProvider;
            _commands = commands;
            _commands.Log += CommandsOnLog;
            _client = client;
        }

        private static Task CommandsOnLog(LogMessage arg)
        {
            File.AppendAllText("commands.log", arg.ToString());

            return Task.FromResult(true);
        }
        
        /// <summary>
        /// install commands as an asynchronous operation.
        /// </summary>
        public async Task InstallCommandsAsync()
        {
            _client.MessageReceived += HandleCommandAsync;
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _serviceProvider);
        }

        /// <summary>
        /// handle command as an asynchronous operation.
        /// </summary>
        /// <param name="messageParam">The message parameter.</param>
        private async Task HandleCommandAsync(SocketMessage messageParam)
        {
            // Don't process the command if it was a system message
            if (messageParam is not SocketUserMessage message) return;

            if (!_databaseService.GetSettings().AllowedChannels.Contains(message.Channel.Id))
            {
                return;
            }

            var argPos = 0;

            // Determine if the message is a command based on the prefix and make sure no bots trigger commands
            if (!(message.HasCharPrefix('#', ref argPos) ||
                message.HasMentionPrefix(_client.CurrentUser, ref argPos)) ||
                message.Author.IsBot)
                return;
            
            // Create a WebSocket-based command context based on the message
            var context = new SocketCommandContext(_client, message);

            // Execute the command with the command context we just
            // created, along with the service provider for precondition checks.

            // Keep in mind that result does not indicate a return value
            // rather an object stating if the command executed successfully.
            var result = await _commands.ExecuteAsync(
                context,
                argPos,
                null);

            //if (!result.IsSuccess)
            //     await context.Channel.SendMessageAsync(result.ErrorReason);
        }
    }
}
