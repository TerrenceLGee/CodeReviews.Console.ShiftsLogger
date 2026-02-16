using System.ComponentModel.DataAnnotations;

namespace ShiftsLogger.Contracts.TerrenceLGee.DTOs.AuthDTOs;

public class UserLogoutDto
{
    [Required(ErrorMessage = "User Id is required.")]
    public string UserId { get; set; } = string.Empty;
}
