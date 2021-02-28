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
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using SupportBot.Checks.Modal;
using System.Text;

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
        public Task CheckSteam([Remainder][Summary("The server ip address or FQDN to check")] string address)
        {
            Context.Message.DeleteAsync();
            
            try
            {                
                var sb = new StringBuilder();
                
                if (!Helpers.CheckIpValid(address))
                {
                    var entries = Dns.GetHostAddresses(address);
                    if (entries.Length > 1)
                    {
                        sb.Append("Multiple addresses found for this host, only the first is checked");
                    }
                    
                    //Set the address to the resolved IP.
                    address = entries[0].ToString();
                }

                using var webClient = new WebClient();
                var result = JsonSerializer.Deserialize<SteamAPIResponse>(webClient.DownloadString($"https://api.steampowered.com/ISteamApps/GetServersAtAddress/v0001?addr={address}"));

                if (!result.response.success) return ReplyAsync("Steam API resulted in a failure. Try again later.");
                
                var totalServers = result.response.servers.Length;

                sb.Append("Steam can see the following:\n");

                foreach (var item in result.response.servers)
                {
                    sb.Append($"**{item.gamedir}**\nApp ID: {item.appid}\nIs Secure: {item.secure}\nIs Lan:{item.lan}\nGame Port:{item.gameport}\nSpec Port:{item.specport}\n");
                }

                sb.Append($"Total Servers: {totalServers}");

                return ReplyAsync(sb.ToString());

            }
            catch (Exception)
            {
                Console.WriteLine("ERROR!");
                // ignored
            }

            return ReplyAsync("Unable to check with steam, sorry about that.");
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
            [Summary("The server ip address or FQDN to check")] string address,
            [Summary("port to check")] int port,
            [Summary("Specify either: `tcp` or `udp`")] string type)
        {
            Context.Message.DeleteAsync();
            
            var result = Helpers.GetPortState(address, port, 2, type.ToLower() == "udp");
           
            return ReplyAsync($"{port}/{type}: {Enum.GetName(result)}");
        }
    }
}