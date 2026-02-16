namespace ShiftsLogger.Client.TerrenceLGee.Data;

public static class Urls
{
    public static string BaseUrl => "https://localhost:7001/api/";
    public static string RegisterUrl => "auth/register";
    public static string LoginUrl => "auth/login";
    public static string LogoutUrl => "auth/logout";
    public static string AddShiftUrl => "shifts/add";
    public static string UpdateShiftUrl => "shifts/update/";
    public static string DeleteShiftUrl => "shifts/delete/";
    public static string GetShiftUrl => "shifts/";
    public static string GetShiftsUrl => "shifts";
    public static string GetAllShiftsForAdminUrl => "shifts/admin";
    public static string GetShiftsCountUrl => "shifts/count";
    public static string GetAllShiftsCountForAdminUrl => "shifts/admin/count";
}
