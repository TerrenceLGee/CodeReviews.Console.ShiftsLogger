using System.ComponentModel.DataAnnotations;

namespace ShiftsLogger.Contracts.TerrenceLGee.DTOs.ShiftDTOs;

public class RetrievedShiftDto
{
    [Required(ErrorMessage = "Shift Id is required.")]
    public int Id { get; set; }

    [Required(ErrorMessage = "User Id is required.")]
    public string UserId { get; set; } = string.Empty;

    [Required(ErrorMessage = "Shift start date and time is required.")]
    public DateTime ShiftStart { get; set; }

    public DateTime? ShiftEnd { get; set; }
    public TimeSpan? Duration { get; set; }
}
