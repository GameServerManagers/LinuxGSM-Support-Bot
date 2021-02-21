// ***********************************************************************
// Assembly         : SupportBot
// Author           : Grimston
// Created          : 01-24-2020
//
// Last Modified By : Grimston
// Last Modified On : 02-20-2021
// ***********************************************************************
// <copyright file="CommandHandler.cs" company="NPipes">
//     Copyright (c) NPipes. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SupportBot
{
    /// <summary>
    /// Class CommandHandler.
    /// </summary>
    public class CommandHandler
    {
        /// <summary>
        /// The client
        /// </summary>
        private readonly DiscordSocketClient _client;
        /// <summary>
        /// The commands
        /// </summary>
        private readonly CommandService _commands;
        /// <summary>
        /// The services
        /// </summary>
        private readonly IServiceProvider _services;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandHandler"/> class.
        /// </summary>
        /// <param name="services">The services.</param>
        public CommandHandler(IServiceProvider services)
        {
            _commands = services.GetRequiredService<CommandService>();
            _client = services.GetRequiredService<DiscordSocketClient>();
            _services = services;
        }

        /// <summary>
        /// install commands as an asynchronous operation.
        /// </summary>
        public async Task InstallCommandsAsync()
        {
            _client.MessageReceived += HandleCommandAsync;
            await _commands.AddModulesAsync(assembly: Assembly.GetEntryAssembly(), services: _services);
        }

        /// <summary>
        /// handle command as an asynchronous operation.
        /// </summary>
        /// <param name="messageParam">The message parameter.</param>
        private async Task HandleCommandAsync(SocketMessage messageParam)
        {
            // Don't process the command if it was a system message
            if (!(messageParam is SocketUserMessage message)) return;

            if (!Worker.Settings.AllowedChannels.Contains(message.Channel.Id))
            {
                return;
            }

            int argPos = 0;

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
                context: context,
                argPos: argPos,
                services: null);

            // Optionally, we may inform the user if the command fails
            // to be executed; however, this may not always be desired,
            // as it may clog up the request queue should a user spam a
            // command.
            // if (!result.IsSuccess)
            //     await context.Channel.SendMessageAsync(result.ErrorReason);
        }
    }
}
