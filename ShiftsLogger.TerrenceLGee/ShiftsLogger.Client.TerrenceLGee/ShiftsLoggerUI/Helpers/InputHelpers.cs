using ShiftsLogger.Client.TerrenceLGee.Extensions;
using ShiftsLogger.Client.TerrenceLGee.Validation;
using ShiftsLogger.Contracts.TerrenceLGee.Enums;
using Spectre.Console;

namespace ShiftsLogger.Client.TerrenceLGee.ShiftsLoggerUI.Helpers;

public static class InputHelpers
{
    private const string DateFormat = "MM-dd-yyyy HH:mm";
    private const string DateFormatSpecifications = "\nThe time should be in 24 hour clock: Hour: (0-23) Minute: (0-59)";

    public static string GetInput(string phrase)
    {
        var validInput = false;
        var input = string.Empty;

        while (!validInput)
        {
            input = AnsiConsole.Ask<string>($"[DarkOliveGreen1]{phrase}: [/]");
            validInput = AnsiConsole.Confirm($"[DarkSeaGreen1]You entered '{input}' is this correct? [/]");
            AnsiConsole.Clear();
        }

        return input;
    }

    public static DateTime? GetValidDateTime(string phrase)
    {
        DateTime? dateTime = null;
        var dateString = string.Empty;

        while (!dateTime.HasValue)
        {
            dateString = AnsiConsole.Ask<string>($"[DarkSeaGreen3_1]{phrase}\nFormat: {DateFormat}\n{DateFormatSpecifications}: [/]");
            dateTime = DateTimeValidation.GetValidDateTime(dateString, DateFormat);
            AnsiConsole.Clear();

            if (!dateTime.HasValue)
            {
                UiHelpers.PressAnyKeyToContinueError("Date string entered was invalid, please try again\n");
            }
        }

        return dateTime;
    }

    public static string GetValidEmailAddress()
    {
        var emailAddress = string.Empty;
        var isValidEmail = false;

        while (!isValidEmail)
        {
            emailAddress = GetInput("Enter email address ");
            isValidEmail = EmailValidation.IsValidEmailAddress(emailAddress);

            if (!isValidEmail)
            {
                UiHelpers
                    .PressAnyKeyToContinue("The email address you entered is not valid, please try again");
            }
        }

        return emailAddress;
    }

    public static string GetValidPassword()
    {
        AnsiConsole.Clear();
        var password = string.Empty;

        var isPasswordConfirmed = false;

        while (!isPasswordConfirmed)
        {
            password = AnsiConsole.Prompt(new TextPrompt<string>("[DarkTurquoise]Enter a password for your account\n" +
                "Password must be at least 8 characters and at least one upper, lower, digit and non alphanumeric character: [/]")
                .Secret());

            if (!PasswordValidation.IsValidPassword(password))
            {
                UiHelpers.PressAnyKeyToContinueError("Password entered is invalid\nPlease try again");
                continue;
            }

            var confirmedPassword = AnsiConsole.Prompt(new TextPrompt<string>("[DarkTurquoise]Re-enter password to confirm: [/]")
                .Secret());

            AnsiConsole.Clear();

            isPasswordConfirmed = password.Equals(confirmedPassword);

            if (!isPasswordConfirmed)
            {
                UiHelpers
                    .PressAnyKeyToContinue("Passwords do not match. Please try again: ");
            }
        }
        return password;
    }

    public static string GetUserPassword()
    {
        return AnsiConsole.Prompt(new TextPrompt<string>("[DarkTurquoise]Enter password: [/]")
            .Secret());
    }

    public static Department GetDepartment()
    {
        return AnsiConsole.Prompt(
            new SelectionPrompt<Department>()
            .Title("[Aquamarine1_1]Please choose your department[/]")
            .AddChoices(Enum.GetValues<Department>())
            .UseConverter(choice => choice.GetDisplayName()));
    }
}
