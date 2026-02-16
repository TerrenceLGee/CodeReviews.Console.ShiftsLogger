using System.ComponentModel.DataAnnotations;

namespace ShiftsLogger.Contracts.TerrenceLGee.DTOs.ShiftDTOs;

public class CreateShiftDto
{
    public string UserId { get; set; } = string.Empty;

    [Required(ErrorMessage = "Shift start date and time is required.")]
    public DateTime ShiftStart { get; set; }

    public DateTime? ShiftEnd { get; set; }
    public TimeSpan? Duration { get; set; }
}
