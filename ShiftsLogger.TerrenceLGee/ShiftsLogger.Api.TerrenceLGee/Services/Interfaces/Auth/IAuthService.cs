using ShiftsLogger.Api.TerrenceLGee.Shared.Results;
using ShiftsLogger.Contracts.TerrenceLGee.DTOs.AuthDTOs;

namespace ShiftsLogger.Api.TerrenceLGee.Services.Interfaces.Auth;

public interface IAuthService
{
    Task<Result> RegisterUserAsync(UserRegistrationDto userDto);
    Task<Result<AuthenticationResponseDto?>> LoginUserAsync(UserLoginDto userDto);
    Task<Result> LogoutUserAsync(UserLogoutDto userDto);
}
