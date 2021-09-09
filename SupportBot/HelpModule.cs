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
using System.Threading.Tasks;
using Discord;

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
        public Task Wsl()
        {
            return ReplyAsync(strings.Wsl);
        }

        [Command("self-update")]
        [RequireUserPermission(GuildPermission.KickMembers)]
        public Task SelfUpdate()
        {
            Task.Run(Worker.UpdateTriggers);

            return ReplyAsync(strings.SelfUpdate);
        }
        
        /// <summary>
        /// Help command to show all available commands
        /// </summary>
        /// <returns>Task.</returns>
        [Command("help")]
        public Task Help()
        {
            return ReplyAsync(strings.Help);
        }
    }
}