using System.ComponentModel.DataAnnotations;

namespace ShiftsLogger.Contracts.TerrenceLGee.DTOs.AuthDTOs;

public class UserLoginDto
{
    [Required(ErrorMessage = "Email address is required.")]
    [EmailAddress(ErrorMessage = "Provided email address is invalid.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required.")]
    public string Password { get; set; } = string.Empty;
}
