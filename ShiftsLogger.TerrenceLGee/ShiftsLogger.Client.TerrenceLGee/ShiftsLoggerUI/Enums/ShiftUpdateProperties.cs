using System.ComponentModel.DataAnnotations;

namespace ShiftsLogger.Client.TerrenceLGee.ShiftsLoggerUI.Enums;

public enum ShiftUpdateProperties
{
    [Display(Name = "Shift Start")]
    ShiftStart,
    [Display(Name = "Shift End")]
    ShiftEnd,
    [Display(Name = "None")]
    None
}
