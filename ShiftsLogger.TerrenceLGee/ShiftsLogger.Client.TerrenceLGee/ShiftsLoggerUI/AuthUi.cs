using ShiftsLogger.Client.TerrenceLGee.Models;
using ShiftsLogger.Client.TerrenceLGee.Services.Interfaces;
using ShiftsLogger.Client.TerrenceLGee.ShiftsLoggerUI.Helpers;
using ShiftsLogger.Client.TerrenceLGee.ShiftsLoggerUI.Interfaces;
using ShiftsLogger.Contracts.TerrenceLGee.DTOs.AuthDTOs;

namespace ShiftsLogger.Client.TerrenceLGee.ShiftsLoggerUI;

public class AuthUi : IAuthUi
{
    private readonly IAuthService _authService;

    public AuthUi(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task RegisterUserAsync()
    {
        var firstName = InputHelpers.GetInput("Enter first name");
        var lastName = InputHelpers.GetInput("Enter last name");
        var email = InputHelpers.GetValidEmailAddress();

        var department = InputHelpers.GetDepartment();

        var password = InputHelpers.GetValidPassword();

        var registration = new UserRegistrationDto
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            Department = department,
            Password = password
        };

        var result = await _authService.RegisterUserAsync(registration);

        if (string.IsNullOrEmpty(result))
        {
            UiHelpers
                .PressAnyKeyToContinueError("Unable to fulfill your registration request at this time");
            return;
        }

        UiHelpers.PressAnyKeyToContinue(result);
    }

    public async Task<AuthData?> LoginAsync()
    {
        var email = InputHelpers.GetInput("Enter email");
        var password = InputHelpers.GetUserPassword();

        var login = new UserLoginDto
        {
            Email = email,
            Password = password
        };

        var result = await _authService.LoginUserAsync(login);

        if (result is null)
        {
            UiHelpers
                .PressAnyKeyToContinueError("Login failed");
            return null;
        }

        if (!string.IsNullOrEmpty(result.ErrorMessage))
        {
            UiHelpers.PressAnyKeyToContinueError(result.ErrorMessage);
            return null;
        }

        UiHelpers.PressAnyKeyToContinue("Login successful");
        return result;
    }

    public async Task LogoutAsync(AuthData authData)
    {
        var result = await _authService.LogoutAsync(authData);

        if (string.IsNullOrEmpty(result))
        {
            UiHelpers
                .PressAnyKeyToContinueError("Logout failed");
            return;
        }

        UiHelpers.PressAnyKeyToContinue(result);
    }
}
