using ShiftsLogger.Api.TerrenceLGee.Models;
using ShiftsLogger.Api.TerrenceLGee.Shared.Pagination;
using ShiftsLogger.Api.TerrenceLGee.Shared.Parameters;

namespace ShiftsLogger.Api.TerrenceLGee.Repositories.Interfaces;

public interface IShiftsLoggerRepository
{
    Task<Shift?> AddShiftAsync(Shift shift);
    Task<Shift?> UpdateShiftAsync(Shift shift);
    Task<bool> DeleteShiftAsync(UserParams userParams);
    Task<Shift?> GetShiftAsync(UserParams userParams);
    Task<PagedList<Shift>> GetShiftsAsync(PaginationParams paginationParams);
    Task<PagedList<Shift>> GetAllShiftsForAdminAsync(PaginationParams paginationParams);
    Task<int> GetCountOfShiftsAsync(UserParams userParams);
    Task<int> GetCountOfAllShiftsForAdminAsync();
    Task<bool> IsValidShiftId(int shiftId);
    Task<bool> IsValidShiftOwnership(UserParams userParams);
}
