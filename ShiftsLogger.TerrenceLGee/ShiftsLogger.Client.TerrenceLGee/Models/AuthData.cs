using System.Text.Json.Serialization;

namespace ShiftsLogger.Client.TerrenceLGee.Models;

public class AuthData
{
    [JsonPropertyName("accessToken")]
    public string AccessToken { get; set; } = string.Empty;

    [JsonPropertyName("refreshToken")]
    public string RefreshToken { get; set; } = string.Empty;

    public string? ErrorMessage { get; set; }
}
