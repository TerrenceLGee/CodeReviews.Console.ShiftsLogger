namespace ShiftsLogger.Api.TerrenceLGee.Models;

public class Shift
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public ApplicationUser User { get; set; } = null!;
    public DateTime ShiftStart { get; set; }
    public DateTime? ShiftEnd { get; set; }
    public TimeSpan? Duration { get; set; }
}
