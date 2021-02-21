// ***********************************************************************
// Assembly         : SupportBot
// Author           : Grimston
// Created          : 01-24-2020
//
// Last Modified By : Grimston
// Last Modified On : 02-20-2021
// ***********************************************************************
// <copyright file="HelpModule.cs" company="NPipes">
//     Copyright (c) NPipes. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Discord.Commands;
using SupportBot.Triggers;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace SupportBot
{
    /// <summary>
    /// Class HelpModule.
    /// Implements the <see cref="ModuleBase{SocketCommandContext}" />
    /// </summary>
    /// <seealso cref="ModuleBase{SocketCommandContext}" />
    public class HelpModule : ModuleBase<SocketCommandContext>
    {
        /// <summary>
        /// cs the worker task.
        /// </summary>
        /// <returns>Task.</returns>
        [Command("cworkthreadpool")]
        public Task CWorkerTask()
        {
            return ReplyAsync(strings.CWorkThreadPool);
        }

        /// <summary>
        /// LVMs the resize.
        /// </summary>
        /// <returns>Task.</returns>
        [Command("lvmsize")]
        public Task LvmResize()
        {
            return ReplyAsync(strings.LvmPartitions);
        }

        /// <summary>
        /// WSLs this instance.
        /// </summary>
        /// <returns>Task.</returns>
        [Command("wsl")]
        public Task WSL()
        {
            return ReplyAsync(strings.Wsl);
        }

        [Command("self-update")]
        public Task SelfUpdate()
        {
            Task.Run(() =>
            {
                using (WebClient client = new WebClient())
                {
                    //BotTrigger triggers  = JsonSerializer.Deserialize<BotTrigger>(client.DownloadString(url));
                    //TODO: Update
                }
            });

            return ReplyAsync(strings.SelfUpdate);
        }
    }
}
