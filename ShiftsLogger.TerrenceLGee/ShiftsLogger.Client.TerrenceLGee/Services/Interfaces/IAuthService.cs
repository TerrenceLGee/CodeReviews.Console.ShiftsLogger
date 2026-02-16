using ShiftsLogger.Client.TerrenceLGee.Models;
using ShiftsLogger.Contracts.TerrenceLGee.DTOs.AuthDTOs;

namespace ShiftsLogger.Client.TerrenceLGee.Services.Interfaces;

public interface IAuthService
{
    Task<string?> RegisterUserAsync(UserRegistrationDto userDto);
    Task<AuthData?> LoginUserAsync(UserLoginDto userDto);
    Task<string?> LogoutAsync(AuthData data);
}
