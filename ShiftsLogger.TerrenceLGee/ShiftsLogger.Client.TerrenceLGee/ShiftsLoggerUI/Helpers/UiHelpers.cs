using Spectre.Console;

namespace ShiftsLogger.Client.TerrenceLGee.ShiftsLoggerUI.Helpers;

public static class UiHelpers
{
    public static void PressAnyKeyToContinue(string? message = "")
    {
        AnsiConsole.MarkupLine($"[Cyan2]{message}[/]");
        AnsiConsole.MarkupLine($"[DarkOliveGreen1]Press any key to continue[/]");
        Console.ReadKey();
        AnsiConsole.Clear();
    }

    public static void PressAnyKeyToContinueError(string? errorMessage)
    {
        AnsiConsole.MarkupLine($"[red]{errorMessage}[/]");
        AnsiConsole.MarkupLine($"[DarkOrange]Press any key to continue[/]");
        Console.ReadKey();
        AnsiConsole.Clear();
    }
}
