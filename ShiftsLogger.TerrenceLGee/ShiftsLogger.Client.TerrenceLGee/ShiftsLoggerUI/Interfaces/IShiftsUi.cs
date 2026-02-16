using ShiftsLogger.Client.TerrenceLGee.Models;

namespace ShiftsLogger.Client.TerrenceLGee.ShiftsLoggerUI.Interfaces;

public interface IShiftsUi
{
    Task AddShiftAsync(AuthData authData);
    Task UpdateShiftAsync(AuthData authData);
    Task DeleteShiftAsync(AuthData authData);
    Task ViewShiftAsync(AuthData authData);
    Task ViewAllShiftsAsync(AuthData authData);
    Task ViewAllShiftsForAdminAsync(AuthData authData);
}
