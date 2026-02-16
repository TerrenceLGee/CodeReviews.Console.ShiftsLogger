using System.ComponentModel.DataAnnotations;

namespace ShiftsLogger.Client.TerrenceLGee.ShiftsLoggerUI.Enums;

public enum ShiftMenu
{
    [Display(Name = "Log a new shift")]
    AddShift,
    [Display(Name = "Update an existing shift")]
    UpdateShift,
    [Display(Name = "Delete an existing shift")]
    DeleteShift,
    [Display(Name = "View a shift by Id")]
    ViewShift,
    [Display(Name = "View all of your shifts")]
    ViewAllShifts,
    [Display(Name = "View all shifts (admin only)")]
    ViewAllShiftsAdmin,
    [Display(Name = "Return to the previous menu")]
    Exit
}
