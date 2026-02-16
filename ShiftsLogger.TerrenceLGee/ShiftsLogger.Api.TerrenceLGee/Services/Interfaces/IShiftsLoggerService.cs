using ShiftsLogger.Contracts.TerrenceLGee.DTOs.ShiftDTOs;
using ShiftsLogger.Api.TerrenceLGee.Shared.Pagination;
using ShiftsLogger.Api.TerrenceLGee.Shared.Parameters;
using ShiftsLogger.Api.TerrenceLGee.Shared.Results;

namespace ShiftsLogger.Api.TerrenceLGee.Services.Interfaces;

public interface IShiftsLoggerService
{
    Task<Result<RetrievedShiftDto?>> AddShiftAsync(CreateShiftDto createShiftDto);
    Task<Result<RetrievedShiftDto?>> UpdateShiftAsync(UpdateShiftDto updateShiftDto);
    Task<Result> DeleteShiftAsync(UserParams userParams);
    Task<Result<RetrievedShiftDto?>> GetShiftAsync(UserParams userParams);
    Task<Result<PagedList<RetrievedShiftDto>>> GetShiftsAsync(PaginationParams paginationParams);
    Task<Result<PagedList<RetrievedShiftDto>>> GetAllShiftsForAdminAsync(PaginationParams paginationParams);
    Task<Result<int>> GetCountOfShiftsAsync(UserParams userParams);
    Task<Result<int>> GetCountOfAllShiftsForAdminAsync();
}
