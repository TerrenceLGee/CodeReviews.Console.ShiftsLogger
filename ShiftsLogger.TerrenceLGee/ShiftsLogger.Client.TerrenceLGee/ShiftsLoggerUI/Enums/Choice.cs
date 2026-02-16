using System.ComponentModel.DataAnnotations;

namespace ShiftsLogger.Client.TerrenceLGee.ShiftsLoggerUI.Enums;

public enum Choice
{
    [Display(Name = "Previous Page")]
    Previous,
    [Display(Name = "Next Page")]
    Next,
    [Display(Name = "Make Choice")]
    MakeChoice,
    [Display(Name = "Finished Viewing")]
    Exit
}
