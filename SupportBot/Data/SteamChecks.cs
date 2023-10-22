using SupportBot.Modules.Modal;

namespace SupportBot.Data;

public class SteamChecks
{
    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    /// <value>The identifier.</value>
    public ulong Id { get; set; }

    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    /// <value>The name.</value>
    public Response SteamResponse { get; set; }
}