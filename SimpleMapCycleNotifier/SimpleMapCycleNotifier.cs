using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using Discord;
using Discord.Webhook;
using Microsoft.Extensions.Logging;

namespace SimpleMapCycleNotifier;

public class SimpleMapCycleNotifier: BasePlugin, IPluginConfig<SmcnConfig>
{
    public override string ModuleName => "SimpleMapCycleNotifier";
    public override string ModuleVersion => "0.0.1";

    public override string ModuleAuthor => "faketuna";

    public override void Load(bool hotReload)
    {
        RegisterListener<Listeners.OnMapStart>(mapName =>
        {
            if (Config.IgnoredMaps.Contains(mapName))
                return;
            
            
            int clients = Utilities.GetPlayers().Count(p => p is { IsBot: false, IsHLTV: false });
            int maxClients = Server.MaxPlayers;
            
            
            Task.Run(async () =>
            {
                using var client = new DiscordWebhookClient(Config.WebHookUrl);

                EmbedBuilder embed = new EmbedBuilder()
                    .WithTitle("POSSESSION")
                    .WithDescription($"{mapName} - {clients}/{maxClients}")
                    .WithColor(Color.Blue)
                    .WithCurrentTimestamp();
                
                await client.SendMessageAsync("", false, [embed.Build()]);
            }).ContinueWith(task =>
            {
                if (task.IsFaulted)
                    Logger.LogError(task.Exception, "Error sending message to discord server");
            });
        });
    }

    
    public void OnConfigParsed(SmcnConfig config)
    {
        Config = config;
    }

    public SmcnConfig Config { get; set; } = null!;
}
