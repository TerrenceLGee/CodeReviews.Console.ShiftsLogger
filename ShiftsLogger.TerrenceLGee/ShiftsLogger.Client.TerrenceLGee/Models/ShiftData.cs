using System.Text.Json.Serialization;

namespace ShiftsLogger.Client.TerrenceLGee.Models;

public class ShiftData
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("userId")]
    public string UserId { get; set; } = string.Empty;

    [JsonPropertyName("shiftStart")]
    public DateTime ShiftStart { get; set; }

    [JsonPropertyName("shiftEnd")]
    public DateTime? ShiftEnd { get; set; }

    [JsonPropertyName("duration")]
    public TimeSpan? Duration { get; set; }

    public string? ErrorMessage { get; set; }
}
