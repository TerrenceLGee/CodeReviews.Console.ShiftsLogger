using System.ComponentModel.DataAnnotations;

namespace ShiftsLogger.Contracts.TerrenceLGee.DTOs.AuthDTOs;

public class AuthenticationResponseDto
{
    [Required(ErrorMessage = "Access token is required.")]
    public string AccessToken { get; set; } = string.Empty;

    [Required(ErrorMessage = "Refresh token is required.")]
    public string RefreshToken { get; set; } = string.Empty;
}
