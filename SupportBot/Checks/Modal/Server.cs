// ***********************************************************************
// Assembly         : SupportBot
// Author           : Nathan Pipes
// Created          : 02-22-2021
//
// Last Modified By : Nathan Pipes
// Last Modified On : 02-22-2021
// ***********************************************************************
// <copyright file="Server.cs" company="NPipes">
//     Copyright (c) NPipes. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace SupportBot.Checks.Modal
{
    /// <summary>
    /// Steam API Server Response.
    /// </summary>
    public class Server
    {
        /// <summary>
        /// Gets or sets the address of the server.
        /// </summary>
        /// <value>The addr.</value>
        public string addr { get; set; }

        /// <summary>
        /// Gets or sets the gmsindex.
        /// </summary>
        /// <value>The gmsindex.</value>
        public int gmsindex { get; set; }

        /// <summary>
        /// Gets or sets the app id of the server.
        /// </summary>
        /// <value>The appid.</value>
        public int appid { get; set; }

        /// <summary>
        /// Gets or sets the gamedir.
        /// </summary>
        /// <value>The gamedir.</value>
        public string gamedir { get; set; }

        /// <summary>
        /// Gets or sets the region.
        /// </summary>
        /// <value>The region.</value>
        public int region { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Server"/> is secure.
        /// </summary>
        /// <value><c>true</c> if secure; otherwise, <c>false</c>.</value>
        public bool secure { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Server"/> is lan.
        /// </summary>
        /// <value><c>true</c> if lan; otherwise, <c>false</c>.</value>
        public bool lan { get; set; }

        /// <summary>
        /// Gets or sets the gameport.
        /// </summary>
        /// <value>The gameport.</value>
        public int gameport { get; set; }

        /// <summary>
        /// Gets or sets the specport.
        /// </summary>
        /// <value>The specport.</value>
        public int specport { get; set; }
    }
}
