using System.Text.Json.Serialization;

namespace ShiftsLogger.Client.TerrenceLGee.Models;

public class RegistrationRoot : Root
{
    [JsonPropertyName("data")]
    public string? Data { get; set; }
}
