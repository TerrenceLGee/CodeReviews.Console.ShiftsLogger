using System.Text.Json.Serialization;

namespace ShiftsLogger.Client.TerrenceLGee.Models;

public class ShiftsRoot : Root
{
    [JsonPropertyName("data")]
    public List<ShiftsData> Data { get; set; } = [];
}
