using System.ComponentModel.DataAnnotations;

namespace ShiftsLogger.Api.TerrenceLGee.Models;

public class RefreshToken
{
    public int Id { get; set; }
    public required string Token { get; set; } = string.Empty;
    public required string JwtId { get; set; } = string.Empty;
    public required DateTime Expires { get; set; }
    public bool IsRevoked { get; set; } = false;
    public DateTime? RevokedAt { get; set; }
    public string UserId { get; set; } = string.Empty;
    public ApplicationUser User { get; set; } = null!;
    public required DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
