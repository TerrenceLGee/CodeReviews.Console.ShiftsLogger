using System.Text.Json.Serialization;

namespace ShiftsLogger.Client.TerrenceLGee.Models;

public class DeletionRoot : Root
{
    [JsonPropertyName("data")]
    public string? Data { get; set; }
}
