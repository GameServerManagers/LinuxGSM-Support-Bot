// ***********************************************************************
// Assembly         : SupportBot
// Author           : Nathan Pipes
// Created          : 02-22-2021
//
// Last Modified By : Nathan Pipes
// Last Modified On : 02-22-2021
// ***********************************************************************
// <copyright file="Response.cs" company="NPipes">
//     Copyright (c) NPipes. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace SupportBot.Modules.Modal
{
    /// <summary>
    /// Steam API Response.
    /// </summary>
    public class Response
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Response"/> is success.
        /// </summary>
        /// <value><c>true</c> if success; otherwise, <c>false</c>.</value>
        public bool success { get; set; }

        /// <summary>
        /// Gets or sets the servers.
        /// </summary>
        /// <value>The servers.</value>
        public Server[] servers { get; set; }
    }
}
