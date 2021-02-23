using System;
using Discord.Commands;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using SupportBot.Checks.Modal;
using System.Text;

namespace SupportBot.Checks
{
    public class CheckModule : ModuleBase<SocketCommandContext>
    {
        [Command("check-steam")]
        public Task CheckSteam([Remainder][Summary("The server ip address or FQDN to check")] string address)
        {
            try
            {
                using var webClient = new WebClient();
                var result = JsonSerializer.Deserialize<SteamAPIResponse>(webClient.DownloadString($"https://api.steampowered.com/ISteamApps/GetServersAtAddress/v0001?addr={address}"));

                if (result.response.success)
                {
                    var totalServers = result.response.servers.Length;

                    var sb = new StringBuilder();
                    sb.Append("Steam can see the following:\n");

                    foreach (var item in result.response.servers)
                    {
                        sb.Append($"**{item.gamedir}**\nApp ID: {item.appid}\nIs Secure: {item.secure}\nIs Lan:{item.lan}\nGame Port:{item.gameport}\nSpec Port:{item.specport}\n");
                    }

                    sb.Append($"Total Servers: {totalServers}");

                    return ReplyAsync(sb.ToString());
                }
                else
                {
                    return ReplyAsync("Steam API resulted in a failure. Try again later.");
                }
            }
            catch (Exception)
            {
                // ignored
            }

            return ReplyAsync("Unable to check with steam, sorry about that.");
        }
        
                [Command("check-port")]
        public Task CheckPort(
            [Summary("The server ip address or FQDN to check")] string address,
            [Summary("port to check")] int port,
            [Summary("Specify either: `tcp` or `udp`")] string type)
        {
            var result = Helpers.GetPortState(address, port, 2, type.ToLower() == "udp");
           
            return ReplyAsync($"{address} {port}/{type}: {Enum.GetName(result)}");
        }
    }
}