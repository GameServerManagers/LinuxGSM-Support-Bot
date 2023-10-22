using System.Net.Http;
using System.Reflection;
using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SupportBot;
using SupportBot.Modules;
using SupportBot.Services;

//TODO: Create a web interface using Blazor components to assist with managing the bot.

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSystemd();
var config = new DiscordSocketConfig()
{
    UseInteractionSnowflakeDate = false
};

builder.Services.AddSingleton(config);
builder.Services.AddSingleton<HttpClient>();
builder.Services.AddSingleton<CheckModule>();
builder.Services.AddSingleton<HelpModule>();
builder.Services.AddSingleton<CommandService>();
builder.Services.AddSingleton<CommandHandler>();
builder.Services.AddSingleton<DiscordSocketClient>();
builder.Services.AddSingleton<InteractionHandler>();
builder.Services.AddSingleton<DatabaseService>();
builder.Services.AddSingleton(x => new InteractionService(x.GetRequiredService<DiscordSocketClient>()));
builder.Services.AddHostedService<Worker>();

builder.Services.AddControllers();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();