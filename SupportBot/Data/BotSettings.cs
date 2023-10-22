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
