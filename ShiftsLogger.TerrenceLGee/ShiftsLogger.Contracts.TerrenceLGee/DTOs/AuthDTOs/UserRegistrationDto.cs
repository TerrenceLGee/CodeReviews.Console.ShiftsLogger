using ShiftsLogger.Contracts.TerrenceLGee.Enums;
using System.ComponentModel.DataAnnotations;

namespace ShiftsLogger.Contracts.TerrenceLGee.DTOs.AuthDTOs;

public class UserRegistrationDto
{
    [Required(ErrorMessage = "First name is required.")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Last name is required.")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Provided email address is invalid.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Department is required.")]
    public Department Department { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    public string Password { get; set; } = string.Empty;
}
