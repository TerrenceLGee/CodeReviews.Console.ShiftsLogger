using System.Text.Json.Serialization;

namespace ShiftsLogger.Client.TerrenceLGee.Models;

public class AuthRoot : Root
{
    [JsonPropertyName("data")]
    public AuthData? Data { get; set; }
}
