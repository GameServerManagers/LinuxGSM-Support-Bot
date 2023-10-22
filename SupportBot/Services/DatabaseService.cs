using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using LiteDB;
using SupportBot.Data;
using SupportBot.Triggers;

namespace SupportBot.Services;

public class DatabaseService : IDisposable
{
    private BotSettings _botSettings;
    
    private LiteDatabase Database { get; set; } = new(Path.Combine(
        (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
            ? Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location)
            : "/srv/LinuxGSMbot/LinuxGSMbot") ?? "/srv/LinuxGSMbot/LinuxGSMbot", "bot.db"));

    public ILiteCollection<BotSettings> Settings()
    {
        return Database.GetCollection<BotSettings>("settings");
    }
    
    public BotSettings GetSettings()
    {
        if (_botSettings == null)
        {
            var collection = Settings();
            
            _botSettings = collection.FindAll().OrderBy(x => x.Id).Last();
        }
        
        return _botSettings;
    }

    public ILiteCollection<SteamChecks> SteamChecks()
    {
        return Database.GetCollection<SteamChecks>();
    }

    public void Dispose()
    {
        Database?.Dispose();
    }

    public ILiteCollection<Trigger> Triggers()
    {
        return Database.GetCollection<Trigger>();
    }
}