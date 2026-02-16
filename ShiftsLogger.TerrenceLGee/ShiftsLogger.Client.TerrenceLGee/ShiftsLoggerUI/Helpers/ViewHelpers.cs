using ShiftsLogger.Client.TerrenceLGee.Models;
using Spectre.Console;

namespace ShiftsLogger.Client.TerrenceLGee.ShiftsLoggerUI.Helpers;

public static class ViewHelpers
{
    public static void DisplayShiftDetails(ShiftData shift)
    {
        var rows = new Rows(
            new Markup($"Shift Id: [Khaki1]{shift.Id}[/]"),
            new Markup($"Shift start: [Khaki1]{shift.ShiftStart}[/]"),
            new Markup($"Shift end: [Khaki1]{((shift.ShiftEnd.HasValue) ? shift.ShiftEnd.Value.ToString() : "N/A")}[/]"),
            new Markup($"Shift duration: [Khaki1]{((shift.Duration.HasValue) ? shift.Duration.Value.ToString() : "N/A")}[/]"));

        AnsiConsole.Write(rows);
    }

    public static void DisplayShifts(List<ShiftsData> shifts)
    {
        var table = new Table()
            .BorderColor(Color.DeepSkyBlue3_1)
            .Title("Shifts");
        table.AddColumn("[Cyan]Shift Id[/]");
        table.AddColumn("[Cyan]Shift Start[/]");
        table.AddColumn("[Cyan]Shift End[/]");
        table.AddColumn("[Cyan]Shift Duration[/]");

        foreach (var shift in shifts)
        {
            table.AddRow(
                $"[Cyan]{shift.Id}[/]",
                $"[Cyan]{shift.ShiftStart}[/]",
                $"[Cyan]{((shift.ShiftEnd.HasValue) ? shift.ShiftEnd.Value.ToString() : "N/A")}[/]",
                $"[Cyan]{((shift.Duration.HasValue) ? shift.Duration.Value.ToString() : "N/A")}[/]");
        }

        AnsiConsole.Write(table);
        AnsiConsole.WriteLine();
    }

    public static void DisplayShiftsForAdmin(List<ShiftsData> shifts)
    {
        var table = new Table()
            .BorderColor(Color.DeepSkyBlue4)
            .Title("Employee Shifts");
        table.AddColumn("[cyan]Shift Id[/]");
        table.AddColumn("[cyan]User Id[/]");
        table.AddColumn("[cyan]Shift Start[/]");
        table.AddColumn("[cyan]Shift End[/]");
        table.AddColumn("[cyan]Shift Duration[/]");

        foreach (var shift in shifts)
        {
            table.AddRow(
                $"[cyan]{shift.Id}[/]",
                $"[cyan]{shift.UserId}[/]",
                $"[cyan]{shift.ShiftStart}[/]",
                $"[cyan]{((shift.ShiftEnd.HasValue) ? shift.ShiftEnd.Value.ToString() : "N/A")}[/]",
                $"[cyan]{((shift.Duration.HasValue) ? shift.Duration.Value.ToString() : "N/A")}[/]");
        }

        AnsiConsole.Write(table);
        AnsiConsole.WriteLine();
    }
}
