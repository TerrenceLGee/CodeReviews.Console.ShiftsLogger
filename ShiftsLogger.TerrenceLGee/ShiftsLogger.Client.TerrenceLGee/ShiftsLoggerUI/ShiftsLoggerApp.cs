using ShiftsLogger.Client.TerrenceLGee.Extensions;
using ShiftsLogger.Client.TerrenceLGee.Models;
using ShiftsLogger.Client.TerrenceLGee.ShiftsLoggerUI.Enums;
using ShiftsLogger.Client.TerrenceLGee.ShiftsLoggerUI.Helpers;
using ShiftsLogger.Client.TerrenceLGee.ShiftsLoggerUI.Interfaces;
using Spectre.Console;

namespace ShiftsLogger.Client.TerrenceLGee.ShiftsLoggerUI;

public class ShiftsLoggerApp
{
    private readonly IAuthUi _authUi;
    private readonly IShiftsUi _shiftsUi;

    public ShiftsLoggerApp(IAuthUi authUi, IShiftsUi shiftsUi)
    {
        _authUi = authUi;
        _shiftsUi = shiftsUi;
    }

    public async Task RunAsync()
    {
        DisplayMessage("Shifts Logger");
        UiHelpers.PressAnyKeyToContinue();

        var isUserFinished = false;
        var authData = new AuthData();

        while (!isUserFinished)
        {
            var authChoice = GetAuthMenuChoice();

            switch (authChoice)
            {
                case AuthMenu.Register:
                    await _authUi.RegisterUserAsync();
                    break;
                case AuthMenu.Login:
                    authData = await _authUi.LoginAsync();
                    if (authData is null)
                    {
                        break;
                    }
                    else
                    {
                        await ShiftChoices(authData);
                    }
                    break;
                case AuthMenu.Logout:
                    await _authUi.LogoutAsync(authData!);
                    break;
                case AuthMenu.Exit:
                    UiHelpers.PressAnyKeyToContinue("\nGoodbye");
                    return;
                default:
                    UiHelpers.PressAnyKeyToContinueError("Invalid choice made");
                    break;
            }
        }
    }

    private async Task ShiftChoices(AuthData authData)
    {
        var isUserFinishedWithShifts = false;

        while (!isUserFinishedWithShifts)
        {
            var shiftChoice = GetShiftMenuChoice();
            switch (shiftChoice)
            {
                case ShiftMenu.AddShift:
                    await _shiftsUi.AddShiftAsync(authData);
                    break;
                case ShiftMenu.UpdateShift:
                    await _shiftsUi.UpdateShiftAsync(authData);
                    break;
                case ShiftMenu.DeleteShift:
                    await _shiftsUi.DeleteShiftAsync(authData);
                    break;
                case ShiftMenu.ViewShift:
                    await _shiftsUi.ViewShiftAsync(authData);
                    break;
                case ShiftMenu.ViewAllShifts:
                    await _shiftsUi.ViewAllShiftsAsync(authData);
                    break;
                case ShiftMenu.ViewAllShiftsAdmin:
                    await _shiftsUi.ViewAllShiftsForAdminAsync(authData);
                    break;
                case ShiftMenu.Exit:
                    return;
                default:
                    break;
            }
        }
    }

    private static AuthMenu GetAuthMenuChoice()
    {
        return AnsiConsole.Prompt(
            new SelectionPrompt<AuthMenu>()
            .Title("[DarkTurquoise]Please choose one of the following[/]")
            .AddChoices(Enum.GetValues<AuthMenu>())
            .UseConverter(choice => choice.GetDisplayName()));
    }

    private static ShiftMenu GetShiftMenuChoice()
    {
        return AnsiConsole.Prompt(
            new SelectionPrompt<ShiftMenu>()
            .Title("[DarkOliveGreen3_2]Please choose one of the following[/]")
            .AddChoices(Enum.GetValues<ShiftMenu>())
            .UseConverter(choice => choice.GetDisplayName()));
    }

    private static void DisplayMessage(string message)
    {
        AnsiConsole.Write(
            new FigletText($"{message}")
            .Centered()
            .Color(Color.DarkSeaGreen2_1));
    }
}
