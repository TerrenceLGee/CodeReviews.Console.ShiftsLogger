using ShiftsLogger.Client.TerrenceLGee.Extensions;
using ShiftsLogger.Client.TerrenceLGee.Models;
using ShiftsLogger.Client.TerrenceLGee.Services.Interfaces;
using ShiftsLogger.Client.TerrenceLGee.ShiftsLoggerUI.Enums;
using ShiftsLogger.Client.TerrenceLGee.ShiftsLoggerUI.Helpers;
using ShiftsLogger.Client.TerrenceLGee.ShiftsLoggerUI.Interfaces;
using ShiftsLogger.Contracts.TerrenceLGee.DTOs.ShiftDTOs;
using Spectre.Console;

namespace ShiftsLogger.Client.TerrenceLGee.ShiftsLoggerUI;

public class ShiftsUi : IShiftsUi
{
    private readonly IShiftsService _shiftsService;

    public ShiftsUi(IShiftsService shiftsService)
    {
        _shiftsService = shiftsService;
    }

    public async Task AddShiftAsync(AuthData authData)
    {
        DateTime? shiftEnd = null;

        var shiftStart = InputHelpers.GetValidDateTime("Enter shift start");

        var isShiftEnded = AnsiConsole.Confirm("Do you wish to enter an end time for shift? ");

        if (isShiftEnded)
        {
            AnsiConsole.Clear();
            shiftEnd = InputHelpers.GetValidDateTime("Enter shift end");
            ShiftsUiHelpers.GetValidShiftTimes(ref shiftStart, ref shiftEnd, true, true);
        }

        var shift = new CreateShiftDto
        {
            ShiftStart = shiftStart!.Value,
            ShiftEnd = (shiftEnd.HasValue)
            ? shiftEnd.Value
            : null,
            Duration = (shiftEnd.HasValue)
            ? shiftEnd.Value - shiftStart
            : null
        };

        var addedShift = await _shiftsService.AddShiftAsync(shift, authData);

        if (addedShift is null)
        {
            UiHelpers.PressAnyKeyToContinueError("There was an error adding the new shift " +
                "\nReturning to the previous menu");
            return;
        }

        if (!string.IsNullOrEmpty(addedShift.ErrorMessage))
        {
            UiHelpers
                .PressAnyKeyToContinueError($"{addedShift.ErrorMessage}" +
                $"\nReturning to the previous menu");
            return;
        }

        ViewHelpers.DisplayShiftDetails(addedShift);

        UiHelpers.PressAnyKeyToContinue("\n\n");
    }

    public async Task UpdateShiftAsync(AuthData authData)
    {
        var wishesToUpdateShift = AnsiConsole.Confirm("[DarkSlateGray1]Update a shift?[/]");

        if (!wishesToUpdateShift)
        {
            UiHelpers
                .PressAnyKeyToContinue("Returning to the previous menu");
            return;
        }

        if (!await ShiftsUiHelpers.ViewShiftsAsync(
            _shiftsService.GetShiftsCountAsync,
            _shiftsService.GetShiftsAsync,
            "updating",
            "update",
            authData,
            true))
        {
            return;
        }

        var id = AnsiConsole
            .Ask<int>($"[DarkSeaGreen3]Enter the id of the shift to update or 0 to return to the previous menu: [/]");

        if (id == 0)
        {
            UiHelpers
                .PressAnyKeyToContinue("Returning to the previous menu");
            return;
        }

        AnsiConsole.Clear();

        wishesToUpdateShift = AnsiConsole.Confirm($"[DarkSeaGreen3]Are you sure you wish to update shift {id}? [/]");

        AnsiConsole.Clear();

        if (!wishesToUpdateShift)
        {
            UiHelpers.PressAnyKeyToContinue("Returning to the previous menu");
            return;
        }

        var shiftToUpdate = await _shiftsService.GetShiftAsync(id, authData);

        if (shiftToUpdate is null)
        {
            UiHelpers
                .PressAnyKeyToContinueError("Unable to update shift at this time, " +
                "there was an error retrieving the shift\n" +
                "Returning to the previous menu");
            return;
        }

        if (!string.IsNullOrEmpty(shiftToUpdate.ErrorMessage))
        {
            UiHelpers
                .PressAnyKeyToContinueError($"{shiftToUpdate.ErrorMessage}\n" +
                $"Returning to the previous menu");
            return;
        }

        AnsiConsole.Clear();

        var propertiesToUpdate = AnsiConsole.Prompt(
            new MultiSelectionPrompt<ShiftUpdateProperties>()
            .Title("[DarkOliveGreen1_1]Select all the properties that you wish to update for this shift[/]")
            .Required()
            .AddChoices(Enum.GetValues<ShiftUpdateProperties>())
            .UseConverter(choice => choice.GetDisplayName()));

        AnsiConsole.Clear();

        if (propertiesToUpdate.Contains(ShiftUpdateProperties.None))
        {
            UiHelpers
                .PressAnyKeyToContinue($"You selected 'None' as your choice" +
                $"\nReturning to the previous menu");
            return;
        }

        var updatedShift = await _shiftsService
            .UpdateShiftAsync(ShiftsUiHelpers.GetUpdatedShiftsTimes(shiftToUpdate, propertiesToUpdate), authData);

        if (updatedShift is null)
        {
            UiHelpers
                .PressAnyKeyToContinueError($"There was an error updating shift {id}" +
                "\nReturning to the previous menu");
            return;
        }

        if (!string.IsNullOrEmpty(updatedShift.ErrorMessage))
        {
            UiHelpers
                .PressAnyKeyToContinueError($"There was an error updating shift {id}: {updatedShift.ErrorMessage}" +
                $"\nReturning to the previous menu");
            return;
        }

        AnsiConsole.MarkupLine($"[DarkKhaki]Shift {id} updated successfully\n\n[/]");

        ViewHelpers.DisplayShiftDetails(updatedShift);

        UiHelpers.PressAnyKeyToContinue("\n\n");
    }

    public async Task DeleteShiftAsync(AuthData authData)
    {
        var wishesToDeleteShift = AnsiConsole.Confirm("[DarkSlateGray1]Delete a shift?[/]");

        if (!wishesToDeleteShift)
        {
            UiHelpers
                .PressAnyKeyToContinue("Returning to the previous menu");
            return;
        }

        if (!await ShiftsUiHelpers.ViewShiftsAsync(
            _shiftsService.GetShiftsCountAsync,
            _shiftsService.GetShiftsAsync,
            "deleting",
            "delete",
            authData,
            true))
        {
            return;
        }

        var id = AnsiConsole
            .Ask<int>($"[DarkSeaGreen3]Enter the id of the shift to delete or 0 to return to the previous menu: [/]");

        AnsiConsole.Clear();

        if (id == 0)
        {
            UiHelpers
                .PressAnyKeyToContinue("Returning to the previous menu");
            return;
        }

        wishesToDeleteShift = AnsiConsole.Confirm($"[DarkSeaGreen3]Are you sure you wish to delete shift {id}? [/]");

        AnsiConsole.Clear();

        if (!wishesToDeleteShift)
        {
            UiHelpers.PressAnyKeyToContinue("Returning to the previous menu");
            return;
        }

        var deletionResult = await _shiftsService.DeleteShiftAsync(id, authData);

        if (string.IsNullOrEmpty(deletionResult))
        {
            UiHelpers
                .PressAnyKeyToContinueError($"Unable to delete shift {id}" +
                $"\nReturning to the previous menu");
            return;
        }

        UiHelpers.PressAnyKeyToContinue(deletionResult);
    }

    public async Task ViewShiftAsync(AuthData authData)
    {
        if (!await ShiftsUiHelpers.ViewShiftsAsync(
            _shiftsService.GetShiftsCountAsync,
            _shiftsService.GetShiftsAsync,
            "viewing",
            "view",
            authData,
            true))
        {
            return;
        }

        var id = AnsiConsole
            .Ask<int>("[DarkSeaGreen3]Enter the id of the shift you wish to view or 0 to return to the previous menu: [/]");

        if (id == 0)
        {
            UiHelpers
                .PressAnyKeyToContinue("Returning to the previous menu");
            return;
        }

        AnsiConsole.Clear();

        var shiftToView = await _shiftsService.GetShiftAsync(id, authData);

        if (shiftToView is null)
        {
            UiHelpers.PressAnyKeyToContinueError("There was an error retrieving the shift for viewing" +
                "\nReturning to the previous menu");
            return;
        }

        if (!string.IsNullOrEmpty(shiftToView.ErrorMessage))
        {
            UiHelpers.PressAnyKeyToContinueError($"There was an error retrieving the shift for viewing: {shiftToView.ErrorMessage}" +
                $"\nReturning to the previous menu");
            return;
        }

        ViewHelpers.DisplayShiftDetails(shiftToView);

        UiHelpers.PressAnyKeyToContinue("\n\n");
    }

    public async Task ViewAllShiftsAsync(AuthData authData)
    {
        if (!await ShiftsUiHelpers.ViewShiftsAsync(
            _shiftsService.GetShiftsCountAsync,
            _shiftsService.GetShiftsAsync,
            "viewing",
            "view",
            authData,
            false))
        {
            return;
        }

        UiHelpers.PressAnyKeyToContinue("\n\n");
    }

    public async Task ViewAllShiftsForAdminAsync(AuthData authData)
    {
        if (!await ShiftsUiHelpers.ViewShiftsAsync(
            _shiftsService.GetAllShiftsCountForAdminAsync,
            _shiftsService.GetAllShiftsForAdminAsync,
            "viewing",
            "view",
            authData,
            false,
            true))
        {
            return;
        }

        UiHelpers.PressAnyKeyToContinue("\n\n");
    }
}
