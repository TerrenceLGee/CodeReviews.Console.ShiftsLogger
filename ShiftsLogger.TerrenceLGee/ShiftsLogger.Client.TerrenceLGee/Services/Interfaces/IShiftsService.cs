using ShiftsLogger.Client.TerrenceLGee.Data;
using ShiftsLogger.Client.TerrenceLGee.Models;
using ShiftsLogger.Contracts.TerrenceLGee.DTOs.ShiftDTOs;

namespace ShiftsLogger.Client.TerrenceLGee.Services.Interfaces;

public interface IShiftsService
{
    Task<ShiftData?> AddShiftAsync(CreateShiftDto shift, AuthData data);
    Task<ShiftData?> UpdateShiftAsync(UpdateShiftDto shift, AuthData data);
    Task<string?> DeleteShiftAsync(int shiftId, AuthData data);
    Task<ShiftData?> GetShiftAsync(int shiftId, AuthData data);
    Task<List<ShiftsData>> GetShiftsAsync(PaginationParams paginationParams, AuthData data);
    Task<List<ShiftsData>> GetAllShiftsForAdminAsync(PaginationParams paginationParams, AuthData data);
    Task<(int, string?)> GetShiftsCountAsync(AuthData data);
    Task<(int, string?)> GetAllShiftsCountForAdminAsync(AuthData data);
}
