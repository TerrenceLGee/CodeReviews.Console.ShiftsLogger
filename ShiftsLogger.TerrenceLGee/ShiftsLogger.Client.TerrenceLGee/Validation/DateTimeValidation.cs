using System.Globalization;

namespace ShiftsLogger.Client.TerrenceLGee.Validation;

public static class DateTimeValidation
{
    public static DateTime? GetValidDateTime(string dateTimeString, string dateTimeFormat)
    {
        DateTime? date = null;

        if (DateTime.TryParseExact(dateTimeString, dateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime validDate))
        {
            return validDate;
        }
        return date;
    }

    public static bool IsValidEndDate(DateTime? start, DateTime? end)
    {
        return start <= end;
    }
}
