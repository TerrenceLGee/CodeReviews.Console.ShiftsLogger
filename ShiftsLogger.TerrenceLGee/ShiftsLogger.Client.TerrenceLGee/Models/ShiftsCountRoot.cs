using System.Text.Json.Serialization;

namespace ShiftsLogger.Client.TerrenceLGee.Models;

public class ShiftsCountRoot : Root
{
    [JsonPropertyName("data")]
    public int Data { get; set; }
}
