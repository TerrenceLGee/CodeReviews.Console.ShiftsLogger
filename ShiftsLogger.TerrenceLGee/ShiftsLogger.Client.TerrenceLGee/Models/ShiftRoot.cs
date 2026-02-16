using System.Text.Json.Serialization;

namespace ShiftsLogger.Client.TerrenceLGee.Models;

public class ShiftRoot : Root
{
    [JsonPropertyName("data")]
    public ShiftData? Data { get; set; }
}
