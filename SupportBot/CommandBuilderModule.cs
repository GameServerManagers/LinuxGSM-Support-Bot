// ***********************************************************************
// Assembly         : SupportBot
// Author           : Grimston
// Created          : 01-25-2020
//
// Last Modified By : Grimston
// Last Modified On : 01-25-2020
// ***********************************************************************
// <copyright file="CommandBuilderModule.cs" company="SupportBot">
//     Copyright (c) NPipes. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SupportBot
{
    /// <summary>
    /// Class CommandBuilderModule.
    /// Implements the <see cref="Discord.Commands.ModuleBase{Discord.Commands.SocketCommandContext}" />
    /// </summary>
    /// <seealso cref="Discord.Commands.ModuleBase{Discord.Commands.SocketCommandContext}" />
    [Group("cb")]
    public class CommandBuilderModule : ModuleBase<SocketCommandContext>
    {
        // ~say hello world -> hello world
        /// <summary>
        /// Chmods the specified username.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns>Task.</returns>
        [Command("chown")]
        [Summary("Creates a chown command for the provided username")]
        public Task Chmod([Remainder] [Summary("The username to use")] string username)
        {
            return ReplyAsync($"```BASH\nchown -R {username}:{username} /home/{username}```");
        }
    }
}
