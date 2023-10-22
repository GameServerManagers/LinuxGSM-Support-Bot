using System.Threading.Tasks;
using Discord.Commands;
using Discord.Interactions;

namespace SupportBot.Modules
{
    /// <summary>
    /// Class CommandBuilderModule.
    /// Implements the <see cref="SocketCommandContext" />
    /// </summary>
    /// <seealso cref="SocketCommandContext" />
    [Discord.Commands.Group("cb")]
    public class CommandBuilderModule : InteractionModuleBase
    {
        // ~say hello world -> hello world
        /// <summary>
        /// Chmods the specified username.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns>Task.</returns>
        [SlashCommand("chown", "Create a chown command")]
        [Discord.Commands.Summary("Creates a chown command for the provided username")]
        public Task Chmod([Remainder] [Discord.Commands.Summary("The username to use")] string username)
        {
            return RespondAsync($"```BASH\nchown -R {username}:{username} /home/{username}```");
        }
    }
}
