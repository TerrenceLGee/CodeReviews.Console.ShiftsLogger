using Microsoft.AspNetCore.Identity;
using ShiftsLogger.Contracts.TerrenceLGee.Enums;

namespace ShiftsLogger.Api.TerrenceLGee.Models;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public Department Department { get; set; }
    public ICollection<Shift> Shifts { get; set; } = [];
    public ICollection<RefreshToken> RefreshTokens { get; set; } = [];
}
