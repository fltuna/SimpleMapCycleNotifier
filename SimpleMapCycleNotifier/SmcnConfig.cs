using System.Text.Json.Serialization;
using CounterStrikeSharp.API.Core;

namespace SimpleMapCycleNotifier;

public class SmcnConfig: BasePluginConfig
{
    [JsonPropertyName("WebHookUrl")] public string WebHookUrl { get; set; } = string.Empty;
    [JsonPropertyName("IgnoredMaps")] public HashSet<string> IgnoredMaps { get; set; } = new();
}