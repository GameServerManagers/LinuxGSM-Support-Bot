// ***********************************************************************
// Assembly         : SupportBot
// Author           : Nathan Pipes
// Created          : 02-22-2021
//
// Last Modified By : Nathan Pipes
// Last Modified On : 02-27-2021
// ***********************************************************************
// <copyright file="CheckModule.cs" company="NPipes">
//     Copyright (c) NPipes. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using Discord.Commands;
using System.Threading.Tasks;

namespace SupportBot.Checks
{
    /// <summary>
    /// Defines all the check commands
    /// Implements the <see>
    ///     <cref>Discord.Commands.ModuleBase{Discord.Commands.SocketCommandContext}</cref>
    /// </see>
    /// </summary>
    /// <seealso>
    ///     <cref>Discord.Commands.ModuleBase{Discord.Commands.SocketCommandContext}</cref>
    /// </seealso>
    public class CheckModule : ModuleBase<SocketCommandContext>
    {
        /// <summary>
        /// Checks steam to see what servers are running.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <returns>Task.</returns>
        [Command("check-steam")]
        public async Task CheckSteam([Remainder] [Summary("The server ip address or FQDN to check")] string address)
        {
            await Context.Message.DeleteAsync();

            var response = await Helpers.CheckSteam(address);
            foreach (var output in response.Split(2048))
            {
                await ReplyAsync(output);
            }
        }

        /// <summary>
        /// Checks status of the port.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <param name="port">The port.</param>
        /// <param name="type">The type.</param>
        /// <returns>Task.</returns>
        [Command("check-port")]
        public Task CheckPort(
            [Summary("The server ip address or FQDN to check")]
            string address,
            [Summary("port to check")] int port,
            [Summary("Specify either: `tcp` or `udp`")]
            string type)
        {
            Context.Message.DeleteAsync();

            var result = Helpers.GetPortState(address, port, 2, type.ToLower() == "udp");

            return ReplyAsync($"{port}/{type}: {Enum.GetName(result)}");
        }
    }
}