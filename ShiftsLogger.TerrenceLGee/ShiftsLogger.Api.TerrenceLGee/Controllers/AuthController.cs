using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShiftsLogger.Contracts.TerrenceLGee.DTOs.AuthDTOs;
using ShiftsLogger.Api.TerrenceLGee.Services.Interfaces.Auth;
using ShiftsLogger.Api.TerrenceLGee.Shared;
using System.Security.Claims;

namespace ShiftsLogger.Api.TerrenceLGee.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<ApiResponse<string>>> Register([FromBody] UserRegistrationDto userDto)
    {
        ApiResponse<string> response;
        var result = await _authService.RegisterUserAsync(userDto);

        if (result.IsFailure)
        {

            if (result.ErrorMessage!.Contains("User already exists"))
            {
                response = new ApiResponse<string>(409, [result.ErrorMessage]);

            }
            else if (result.ErrorMessage!.Contains("Unable to register user"))
            {
                response = new ApiResponse<string>(400, [result.ErrorMessage]);
            }
            else
            {
                response = new ApiResponse<string>(500, [result.ErrorMessage]);
            }
            return StatusCode(response.StatusCode, response);
        }

        response = new ApiResponse<string>(200, "Registration successful");
        return StatusCode(response.StatusCode, response);
    }

    [HttpPost("login")]
    public async Task<ActionResult<ApiResponse<AuthenticationResponseDto?>>> Login([FromBody] UserLoginDto loginDto)
    {
        ApiResponse<AuthenticationResponseDto?> response;
        var result = await _authService.LoginUserAsync(loginDto);

        if (result.IsFailure)
        {
            if (result.ErrorMessage!.Contains("User not found"))
            {
                response = new ApiResponse<AuthenticationResponseDto?>(404, [result.ErrorMessage]);
            }
            else if (result.ErrorMessage!.Contains("Login failed"))
            {
                response = new ApiResponse<AuthenticationResponseDto?>(400, [result.ErrorMessage]);
            }
            else
            {
                response = new ApiResponse<AuthenticationResponseDto?>(500, [result.ErrorMessage]);
            }
            return StatusCode(response.StatusCode, response);
        }

        response = new ApiResponse<AuthenticationResponseDto?>(200, result.Value);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<ActionResult<ApiResponse<string>>> Logout()
    {
        ApiResponse<string> response;
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId is null)
        {
            response = new ApiResponse<string>(404, ["User not found"]);
            return StatusCode(response.StatusCode, response);
        }

        var result = await _authService.LogoutUserAsync(new UserLogoutDto { UserId = userId });

        if (result.IsFailure)
        {
            if (result.ErrorMessage!.Contains("Invalid authorization"))
            {
                response = new ApiResponse<string>(401, [result.ErrorMessage]);
            }
            else
            {
                response = new ApiResponse<string>(500, [result.ErrorMessage]);
            }
            return StatusCode(response.StatusCode, response);
        }

        response = new ApiResponse<string>(200, "Logout successful");
        return StatusCode(response.StatusCode, response);
    }
}
