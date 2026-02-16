using System.Text.Json.Serialization;

namespace ShiftsLogger.Client.TerrenceLGee.Models;

public abstract class Root
{
    [JsonPropertyName("statusCode")]
    public int StatusCode { get; set; }

    [JsonPropertyName("success")]
    public bool Success { get; set; }

    [JsonPropertyName("errors")]
    public List<string> Errors { get; set; } = [];

    public string? ErrorMessage { get; set; }
}
