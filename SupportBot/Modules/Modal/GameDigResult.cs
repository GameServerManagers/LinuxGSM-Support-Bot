namespace SupportBot.Modules.Modal;

public class GameDigResult
{
    public string name { get; set; }
    public string map { get; set; }
    public bool password { get; set; }
    public Raw raw { get; set; }
    public int maxplayers { get; set; }
    public Players[] players { get; set; }
    public object[] bots { get; set; }
    public string connect { get; set; }
    public int ping { get; set; }
}

public class Raw
{
    public int protocol { get; set; }
    public string folder { get; set; }
    public string game { get; set; }
    public int appId { get; set; }
    public int numplayers { get; set; }
    public int numbots { get; set; }
    public string listentype { get; set; }
    public string environment { get; set; }
    public int secure { get; set; }
    public string version { get; set; }
    public string steamid { get; set; }
    public string[] tags { get; set; }
}

public class Players
{
    public string name { get; set; }
    public Raw1 raw { get; set; }
}

public class Raw1
{
    public int score { get; set; }
    public double time { get; set; }
}