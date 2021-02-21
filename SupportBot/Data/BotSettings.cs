// ***********************************************************************
// Assembly         : SupportBot
// Author           : Grimston
// Created          : 04-04-2020
//
// Last Modified By : Grimston
// Last Modified On : 02-20-2021
// ***********************************************************************
// <copyright file="BotSettings.cs" company="NPipes">
//     Copyright (c) NPipes. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace SupportBot.Data
{
    /// <summary>
    /// Class BotSettings.
    /// </summary>
    public class BotSettings
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the allowed channels.
        /// </summary>
        /// <value>The allowed channels.</value>
        public ulong[] AllowedChannels { get; set; }
    }
}
