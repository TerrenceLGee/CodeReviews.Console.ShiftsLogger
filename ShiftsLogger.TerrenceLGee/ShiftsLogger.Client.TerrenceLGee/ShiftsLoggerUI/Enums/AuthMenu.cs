using System.ComponentModel.DataAnnotations;

namespace ShiftsLogger.Client.TerrenceLGee.ShiftsLoggerUI.Enums;

public enum AuthMenu
{
    [Display(Name = "Register a new user in the system")]
    Register,
    [Display(Name = "Login")]
    Login,
    [Display(Name = "Logout")]
    Logout,
    [Display(Name = "Exit program")]
    Exit
}
