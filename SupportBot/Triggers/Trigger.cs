// ***********************************************************************
// Assembly         : SupportBot
// Author           : Nathan Pipes
// Created          : 02-20-2021
//
// Last Modified By : Nathan Pipes
// Last Modified On : 02-20-2021
// ***********************************************************************
// <copyright file="Trigger.cs" company="NPipes">
//     Copyright (c) NPipes. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections.Generic;

namespace SupportBot.Triggers
{
    /// <summary>
    /// Class Trigger.
    /// </summary>
    public class Trigger
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
        /// Gets or sets the starters.
        /// </summary>
        /// <value>The starters.</value>
        public List<Starter> Starters { get; set; }
        /// <summary>
        /// Gets or sets the answer.
        /// </summary>
        /// <value>The answer.</value>
        public string Answer { get; set; }
    }
}
