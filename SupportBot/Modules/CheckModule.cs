﻿using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using SupportBot.Data;
using SupportBot.Services;

namespace SupportBot.Modules
{
    public class CheckModule : InteractionModuleBase<SocketInteractionContext>
    {
        private readonly DatabaseService _databaseService;

        public InteractionService Commands { get; set; }

        private InteractionHandler _handler;

        public CheckModule(InteractionHandler handler, DatabaseService databaseService)
        {
            _handler = handler;
            _databaseService = databaseService;
        }

        /// <summary>
        /// Checks what game servers steam can see your server is running
        /// </summary>
        /// <param name="address">The address.</param>
        /// <returns>Task.</returns>
        [SlashCommand("steam", "Checks what game servers steam can see your server is running", true,
            RunMode.Async)]
        public async Task CheckSteam(
            [Summary(description: "The server ip address or FQDN to check")]
            string address)
        {
            await Context.Interaction.DeferAsync(true);

            var response = await Helpers.CheckSteam(address);

            if (response == null)
            {
                await FollowupAsync("No response from steam, try again later", ephemeral: true);
            }
            else
            {
                var steamCollection = _databaseService.SteamChecks();

                var embed = new EmbedBuilder();
                embed.WithImageUrl(
                        $"https://cdn.cloudflare.steamstatic.com/steam/apps/{response.servers[0].appid}/header.jpg")
                    .AddField("Game", response.servers[0].gamedir, true)
                    .AddField("Address", response.servers[0].addr, true)
                    .AddField("App ID",
                        $"[{response.servers[0].appid}](https://steamdb.info/app/{response.servers[0].appid})", true)
                    .WithFooter($"Server 1 of {response.servers.Length}")
                    .WithColor(Color.Green);

                if (response.servers.Length > 1)
                {
                    var menuBuilder = new SelectMenuBuilder()
                        .WithPlaceholder("Select a server")
                        .WithCustomId("ss-s")
                        .WithMinValues(1)
                        .WithMaxValues(1);

                    for (var index = 0; index < response.servers.Length; index++)
                    {
                        var server = response.servers[index];

                        menuBuilder.AddOption($"[{server.gamedir}]: {server.addr}", index.ToString(),
                            isDefault: 0 == index);
                    }

                    var builder = new ComponentBuilder()
                        .WithSelectMenu(menuBuilder);

                    var sentResponse = await FollowupAsync(embed: embed.Build(), components: builder.Build(),
                        ephemeral: true);

                    //Store the data for interactivity
                    steamCollection.Insert(new SteamChecks()
                    {
                        Id = sentResponse.Id,
                        SteamResponse = response
                    });
                }
                else
                {
                    await FollowupAsync(embed: embed.Build(), ephemeral: true);
                }
            }
        }

        [SlashCommand("gamedig", "Runs a gamedig query on the specified server")]
        public async Task GameDig(
            [Choice("Valheim", "valheim")]
            [Choice("Counter-Strike: Global Offensive", "csgo")]
            [Choice("Counter-Strike 2", "cs2")]
            [Choice("Project Zomboid", "przomboid")]
            [Choice("Ark: Survival Evolved", "arkse")]
            [Choice("Minecraft", "minecraft")]
            [Choice("Teamspeak 3", "teamspeak3")]
            [Choice("Rust", "rust")]
            [Choice("7 Days to Die", "7d2d")]
            [Choice("Minecraft: Bedrock Edition", "minecraftpe")]
            [Choice("Garry's Mod", "garrysmod")]
            [Choice("Team Fortress 2", "tf2")]
            [Choice("Left 4 Dead 2 (2009)", "left4dead2")]
            [Choice("Counter-Strike 1.6", "cs16")]
            [Choice("Counter-Strike: Source", "css")]
            [Choice("DayZ", "dayz")]
            [Choice("Terraria", "terraria")]
            [Choice("ARMA 3", "arma3")]
            [Choice("Assetto Corsa", "assettocorsa")]
            [Choice("Unreal Tournament", "ut")]
            [Choice("Core Keeper", "corekeeper")]
            [Choice("Insurgency: Sandstorm", "insurgencysandstorm")]
            [Choice("Squad", "squad")]
            [Choice("Insurgency", "insurgency")]
            [Choice("Killing Floor 2", "killingfloor2")]
            string type,
            [Summary(description: "The server ip address or FQDN to check")]
            string address)
        {
            await DeferAsync(true);

            try
            {
                var result = await Helpers.GameDig(type, address);

                var embed = new EmbedBuilder();

                embed.WithTitle(result.name)
                    .AddField("Map", result.map, true)
                    .AddField("Players", $"{result.players.Length}/{result.maxplayers}", true)
                    .AddField("Ping", result.ping, true)
                    .WithFooter($"Connect {result.connect}")
                    .WithColor(Color.Blue);

                await FollowupAsync(embed: embed.Build(), ephemeral: true);
            }
            catch (Exception exc)
            {
                await FollowupAsync($"Error, please try again later: {exc}");
            }
        }
        
        [SlashCommand("gamedig2", "Runs a gamedig query on the specified server")]
        public async Task GameDigType(
            [Summary(description: "https://github.com/gamedig/node-gamedig/blob/HEAD/GAMES_LIST.md")]
            string type,
            [Summary(description: "The server ip address or FQDN to check")]
            string address)
        {
            await DeferAsync(true);

            try
            {
                var result = await Helpers.GameDig(type, address);

                var embed = new EmbedBuilder();

                embed.WithTitle(result.name)
                    .AddField("Map", result.map, true)
                    .AddField("Players", $"{result.players.Length}/{result.maxplayers}", true)
                    .AddField("Ping", result.ping, true)
                    .WithFooter($"Connect {result.connect}")
                    .WithColor(Color.Blue);

                await FollowupAsync(embed: embed.Build(), ephemeral: true);
            }
            catch (Exception exc)
            {
                await FollowupAsync($"Error, please try again later: {exc}");
            }
        }

        [ComponentInteraction("ss-s")]
        public async Task ProcessSteamCheckServerSelection(string[] selectedServer)
        {
            await DeferAsync(true);

            var serverIndex = Convert.ToInt32(selectedServer[0]);
            var steamCollection = _databaseService.SteamChecks();

            try
            {
                var steamChecks =
                    steamCollection.FindOne(x => x.Id == ((SocketMessageComponent)Context.Interaction).Message.Id);

                var menuBuilder = new SelectMenuBuilder()
                    .WithPlaceholder("Select a server")
                    .WithCustomId("ss-s")
                    .WithMinValues(1)
                    .WithMaxValues(1);
                for (var index = 0; index < steamChecks.SteamResponse.servers.Length; index++)
                {
                    var server = steamChecks.SteamResponse.servers[index];

                    menuBuilder.AddOption($"[{server.gamedir}]: {server.addr}", index.ToString(),
                        isDefault: Convert.ToInt32(selectedServer[0]) == index);
                }

                var builder = new ComponentBuilder().WithSelectMenu(menuBuilder);

                var embed = new EmbedBuilder();

                embed.WithImageUrl(
                        $"https://cdn.cloudflare.steamstatic.com/steam/apps/{steamChecks.SteamResponse.servers[serverIndex].appid}/header.jpg")
                    .AddField("Game", steamChecks.SteamResponse.servers[serverIndex].gamedir, true)
                    .AddField("Address", steamChecks.SteamResponse.servers[serverIndex].addr, true)
                    .AddField("App ID",
                        $"[{steamChecks.SteamResponse.servers[serverIndex].appid}](https://steamdb.info/app/{steamChecks.SteamResponse.servers[serverIndex].appid})",
                        true)
                    .WithFooter($"Server {serverIndex + 1} of {steamChecks.SteamResponse.servers.Length}")
                    .WithColor(Color.Green);

                await ModifyOriginalResponseAsync(msg =>
                {
                    msg.Embed = embed.Build();
                    msg.Components = builder.Build();
                });
            }
            catch (Exception)
            {
                await ModifyOriginalResponseAsync(msg =>
                {
                    msg.Content = "Details have been purged, please run the check again.";
                });
            }
        }

        /// <summary>
        /// Checks status of the port.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <param name="port">The port.</param>
        /// <param name="type">The type.</param>
        /// <returns>Task.</returns>
        [SlashCommand("portscan",
            "Scans the specified server's port using the specified type, UDP cannot be reliably checked")]
        public Task PortScan(
            [Summary(description: "The server ip address or FQDN to check")]
            string address,
            [Summary(description: "port to check")]
            int port,
            [Summary(description: "Specify either: `tcp` or `udp`")]
            string type)
        {
            Context.Interaction.DeferAsync(true);

            var result = Helpers.GetPortState(address, port, 2, type.ToLower() == "udp");

            return FollowupAsync($"{port}/{type}: {Enum.GetName(result)}", ephemeral: true);
        }
    }
}