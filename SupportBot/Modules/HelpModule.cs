using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.Interactions;
using SupportBot.Services;
using SupportBot.Triggers;

namespace SupportBot.Modules
{
    /// <summary>
    /// Class HelpModule.
    /// Implements the <see cref="ModuleBase{SocketCommandContext}" />
    /// </summary>
    /// <seealso cref="ModuleBase{SocketCommandContext}" />
    public class HelpModule : InteractionModuleBase
    {
        private readonly DatabaseService _databaseService;

        public HelpModule(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }
        
        /// <summary>
        /// cs the worker task.
        /// </summary>
        /// <returns>Task.</returns>
        [SlashCommand("cworkthreadpool", "CWorkThreadPool (Ignore it)")]
        public Task CWorkerTask()
        {
            return RespondAsync(strings.CWorkThreadPool, ephemeral: true);
        }

        /// <summary>
        /// LVMs the resize.
        /// </summary>
        /// <returns>Task.</returns>
        [SlashCommand("lvmsize", "Resize an LVM volume")]
        public Task LvmResize()
        {
            return RespondAsync(strings.LvmPartitions, ephemeral: true);
        }

        /// <summary>
        /// WSLs this instance.
        /// </summary>
        /// <returns>Task.</returns>
        [SlashCommand("wsl", "Windows Subsystem for Linux")]
        public Task Wsl()
        {
            return RespondAsync(strings.Wsl, ephemeral: true);
        }

        [SlashCommand("self-update", "Request the bot run an update")]
        [Discord.Commands.RequireUserPermission(GuildPermission.KickMembers)]
        public async Task SelfUpdate()
        {
            try
            {
                await DeferAsync(true);
                using var client = new WebClient();
                var triggers = System.Text.Json.JsonSerializer.Deserialize<BotTrigger>(client.DownloadString(
                    "https://raw.githubusercontent.com/Grimston/LGSM-SupportBot/master/SupportBot/triggers.json"));
                if (triggers == null)
                {
                    await FollowupAsync("Could not update triggers", ephemeral: true);
                    return;
                }

                var triggerCollection = _databaseService.Triggers();
                triggerCollection.DeleteAll(); //Remove everything
                triggerCollection.InsertBulk(triggers.Triggers);
            }
            catch (Exception)
            {
                Debug.Print("Failed to update Triggers!");
                //Usually this is a temporary issue with GitHub.
            }

            await FollowupAsync(strings.SelfUpdate, ephemeral: true);
        }
        
        /// <summary>
        /// Help command to show all available commands
        /// </summary>
        /// <returns>Task.</returns>
        [SlashCommand("help", "Shows the help message")]
        public Task Help()
        {
            return RespondAsync(strings.Help, ephemeral: true);
        }
    }
}