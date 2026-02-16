using ShiftsLogger.Client.TerrenceLGee.Data;
using ShiftsLogger.Client.TerrenceLGee.Models;
using ShiftsLogger.Client.TerrenceLGee.ShiftsLoggerUI.Enums;
using ShiftsLogger.Client.TerrenceLGee.Validation;
using ShiftsLogger.Contracts.TerrenceLGee.DTOs.ShiftDTOs;
using Spectre.Console;

namespace ShiftsLogger.Client.TerrenceLGee.ShiftsLoggerUI.Helpers;

public static class ShiftsUiHelpers
{
    public static async Task<bool> ViewShiftsAsync(
        Func<AuthData, Task<(int, string?)>> getShiftCountAsync,
        Func<PaginationParams, AuthData, Task<List<ShiftsData>>> getShiftsAsync,
        string purpose,
        string errorPurpose,
        AuthData authData,
        bool forChoosing,
        bool isAdmin = false)
    {
        AnsiConsole.Clear();

        var (totalCount, errorMessage) = await getShiftCountAsync(authData);

        if (!string.IsNullOrEmpty(errorMessage))
        {
            UiHelpers
                .PressAnyKeyToContinueError(errorMessage);
            return false;
        }

        if (totalCount == 0)
        {
            UiHelpers
                .PressAnyKeyToContinueError($"No shifts available to {errorPurpose}\n" +
                $"Returning to the previous menu");
            return false;
        }

        var pageSize = AnsiConsole
            .Ask<int>($"[DarkTurquoise]There are {totalCount} shifts available for {purpose}, " +
            $"how many would you like to view at a time? [/]");
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        var page = 1;
        AnsiConsole.Clear();

        while (true)
        {
            var paginationParams = new PaginationParams { Page = page, PageSize = pageSize };
            var shifts = await getShiftsAsync(paginationParams, authData);

            if (shifts.Count == 0)
            {
                UiHelpers
                    .PressAnyKeyToContinueError($"No shifts available to {errorPurpose}\n" +
                    $"Returning to the previous menu");
                return false;
            }

            if (isAdmin)
            {
                ViewHelpers.DisplayShiftsForAdmin(shifts);
            }
            else
            {
                ViewHelpers.DisplayShifts(shifts);
            }

            page = DisplayPrompt(shifts, page, totalPages, forChoosing);

            if (page == -1) break;
        }
        return true;
    }

    private static int DisplayPrompt(
        List<ShiftsData> shifts, 
        int page, 
        int totalPages, 
        bool forChoosing = false)
    {
        AnsiConsole.MarkupLine($"[DarkSeaGreen1_1]Displaying page {page} of {totalPages}\n[/]");

        var prompt = new SelectionPrompt<Choice>()
            .Title("Navigate pages");

        if (page > 1)
        {
            prompt.AddChoice(Choice.Previous);
        }
        if (page < totalPages)
        {
            prompt.AddChoice(Choice.Next);
        }

        if (forChoosing)
        {
            prompt.AddChoice(Choice.MakeChoice);
        }
        else
        {
            prompt.AddChoice(Choice.Exit);
        }

        var choice = AnsiConsole.Prompt(prompt);

        if (choice == Choice.Next && page < totalPages)
        {
            page++;
            AnsiConsole.Clear();
            return page;
        }
        else if (choice == Choice.Previous && page > 1)
        {
            page--;
            AnsiConsole.Clear();
            return page;
        }
        else
        {
            return -1;
        }
    }
    public static UpdateShiftDto GetUpdatedShiftsTimes(ShiftData shift, List<ShiftUpdateProperties> properties)
    {
        DateTime? updatedShiftStart = null;
        DateTime? updatedShiftEnd = null;

        foreach (var prop in properties)
        {
            switch (prop)
            {
                case ShiftUpdateProperties.ShiftStart:
                    updatedShiftStart = InputHelpers.GetValidDateTime("Enter updated shift start");
                    break;
                case ShiftUpdateProperties.ShiftEnd:
                    updatedShiftEnd = InputHelpers.GetValidDateTime("Enter updated shift end");
                    break;
            }
        }

        if (updatedShiftStart.HasValue && updatedShiftEnd.HasValue)
        {
            GetValidShiftTimes(ref updatedShiftStart, ref updatedShiftEnd, true, true);
        }

        if (!updatedShiftStart.HasValue && updatedShiftEnd.HasValue)
        {
            updatedShiftStart = shift.ShiftStart;
            GetValidShiftTimes(ref updatedShiftStart, ref updatedShiftEnd, false, true);
        }

        if (updatedShiftStart.HasValue && !updatedShiftEnd.HasValue && shift.ShiftEnd.HasValue)
        {
            updatedShiftEnd = shift.ShiftEnd.Value;
            GetValidShiftTimes(ref updatedShiftStart, ref updatedShiftEnd, true, false);
        }

        var updatedShift = new UpdateShiftDto
        {
            Id = shift.Id,
            ShiftStart = updatedShiftStart ?? shift.ShiftStart,
            ShiftEnd = updatedShiftEnd ?? shift.ShiftEnd
        };

        updatedShift.Duration = updatedShift.ShiftEnd - updatedShift.ShiftStart;

        return updatedShift;
    }

    public static void GetValidShiftTimes(
        ref DateTime? shiftStart, 
        ref DateTime? shiftEnd, 
        bool updateStart = false, 
        bool updateEnd = false)
    {
        while (!DateTimeValidation.IsValidEndDate(shiftStart, shiftEnd))
        {
            UiHelpers.PressAnyKeyToContinueError($"Shift end must come after shift start\n" +
                $"Currently for shift start you have: {shiftStart}\n" +
                $"Currently for shift end you have: {shiftEnd}");

            if (updateStart)
            {
                shiftStart = InputHelpers.GetValidDateTime("Enter shift start");
            }
            
            if (updateEnd)
            {
                shiftEnd = InputHelpers.GetValidDateTime("Enter shift end");
            }
            
            AnsiConsole.Clear();
        }
    }
}
