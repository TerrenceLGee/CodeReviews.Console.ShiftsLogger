using Microsoft.EntityFrameworkCore;
using ShiftsLogger.Api.TerrenceLGee.Data;
using ShiftsLogger.Api.TerrenceLGee.Models;
using ShiftsLogger.Api.TerrenceLGee.Repositories.Interfaces;
using ShiftsLogger.Api.TerrenceLGee.Shared.Extensions;
using ShiftsLogger.Api.TerrenceLGee.Shared.Pagination;
using ShiftsLogger.Api.TerrenceLGee.Shared.Parameters;

namespace ShiftsLogger.Api.TerrenceLGee.Repositories;

public class ShiftsLoggerRepository : IShiftsLoggerRepository
{
    private readonly ShiftsLoggerDbContext _context;
    private readonly ILogger<ShiftsLoggerRepository> _logger;
    private string _errorMessage = string.Empty;

    public ShiftsLoggerRepository(
        ShiftsLoggerDbContext context, 
        ILogger<ShiftsLoggerRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Shift?> AddShiftAsync(Shift shift)
    {
        try
        {
            await _context.Shifts.AddAsync(shift);
            await _context.SaveChangesAsync();

            return shift;
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(ShiftsLoggerRepository)}\n" +
                $"Method: {nameof(AddShiftAsync)}\n" +
                $"There was an unexpected error adding the shift for user with Id {shift.UserId}: {ex.Message}";
            _logger.LogError(ex, "{msg}\n\n", _errorMessage);
            return null;
        }
    }

    public async Task<Shift?> UpdateShiftAsync(Shift shift)
    {
        try
        {
            var shiftToUpdate = await _context.Shifts
                .FirstOrDefaultAsync(s => s.Id == shift.Id && s.UserId.Equals(shift.UserId));

            if (shiftToUpdate is null) return null;

            shiftToUpdate.ShiftStart = shift.ShiftStart;
            shiftToUpdate.ShiftEnd = shift.ShiftEnd;
            shiftToUpdate.Duration = shift.Duration;

            await _context.SaveChangesAsync();

            return shiftToUpdate;
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(ShiftsLoggerRepository)}\n" +
                $"Method: {nameof(UpdateShiftAsync)}\n" +
                $"There was an unexpected error updating shift {shift.Id} for user {shift.UserId}: {ex.Message}";
            _logger.LogError(ex, "{msg}\n\n", _errorMessage);
            return null;
        }
    }

    public async Task<bool> DeleteShiftAsync(UserParams userParams)
    {
        try
        {
            var shiftToDelete = await _context.Shifts
                .FirstOrDefaultAsync(s => s.Id == userParams.ShiftId && s.UserId.Equals(userParams.UserId));

            if (shiftToDelete is null) return false;

            _context.Shifts.Remove(shiftToDelete);
            await _context.SaveChangesAsync();

            return true;
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(ShiftsLoggerRepository)}\n" +
                $"Method: {nameof(DeleteShiftAsync)}\n" +
                $"There was an unexpected error deleting the shift {userParams.ShiftId} for user {userParams.UserId}: {ex.Message}";
            _logger.LogError(ex, "{msg}\n\n", _errorMessage);
            return false;
        }
    }

    public async Task<Shift?> GetShiftAsync(UserParams userParams)
    {
        try
        {
            var shift = await _context.Shifts
                .FirstOrDefaultAsync(s => s.Id == userParams.ShiftId && s.UserId.Equals(userParams.UserId));

            if (shift is null) return null;

            return shift;
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(ShiftsLoggerRepository)}\n" +
                $"Method: {nameof(GetShiftAsync)}\n" +
                $"There was an unexpected error retrieving the shift {userParams.ShiftId} for user with Id {userParams.UserId}: {ex.Message}";
            _logger.LogError(ex, "{msg}\n\n", _errorMessage);
            return null;
        }
    }

    public async Task<PagedList<Shift>> GetShiftsAsync(PaginationParams paginationParams)
    {
        try
        {
            var count = await GetCountOfShiftsAsync(new UserParams
            {
                UserId = paginationParams.UserId,
                ShiftId = paginationParams.ShiftId
            });

            return await _context.Shifts
                .Where(s => s.UserId.Equals(paginationParams.UserId))
                .AsNoTracking()
                .ToPagedListAsync(count, paginationParams.Page, paginationParams.PageSize);
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(ShiftsLoggerRepository)}\n" +
                $"Method: {nameof(GetShiftsAsync)}\n" +
                $"There was an unexpected error retrieving the shifts for user with Id {paginationParams.UserId}: {ex.Message}";
            _logger.LogError(ex, "{msg}\n\n", _errorMessage);
            return [];
        }
    }

    public async Task<PagedList<Shift>> GetAllShiftsForAdminAsync(PaginationParams paginationParams)
    {
        try
        {
            var count = await GetCountOfAllShiftsForAdminAsync();

            return await _context.Shifts
                .AsNoTracking()
                .OrderBy(s => s.Id)
                .ThenBy(s => s.UserId)
                .ToPagedListAsync(count, paginationParams.Page, paginationParams.PageSize);
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(ShiftsLoggerRepository)}\n" +
                $"Method: {nameof(GetAllShiftsForAdminAsync)}\n" +
                $"There was an unexpected error retrieving all of the shifts: {ex.Message}";
            _logger.LogError(ex, "{msg}\n\n", _errorMessage);
            return [];
        }
    }

    public async Task<int> GetCountOfShiftsAsync(UserParams userParams)
    {
        try
        {
            return await _context.Shifts
                .Where(s => s.UserId.Equals(userParams.UserId))
                .AsNoTracking()
                .CountAsync();
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(ShiftsLoggerRepository)}\n" +
                $"Method: {nameof(GetCountOfShiftsAsync)}\n" +
                $"There was an unexpected error retrieving the total count of your shifts: {ex.Message}";
            _logger.LogError(ex, "{msg}\n\n", _errorMessage);
            return -1;
        }
    }

    public async Task<int> GetCountOfAllShiftsForAdminAsync()
    {
        try
        {
            return await _context.Shifts
                .AsNoTracking()
                .CountAsync();
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(ShiftsLoggerRepository)}\n" +
                $"Method: {nameof(GetCountOfAllShiftsForAdminAsync)}\n" +
                $"There was an unexpected error retrieving the total count of all shifts: {ex.Message}";
            _logger.LogError(ex, "{msg}\n\n", _errorMessage);
            return -1;
        }
    }

    public async Task<bool> IsValidShiftId(int shiftId)
    {
        try
        {
            return await _context.Shifts
                .AsNoTracking()
                .AnyAsync(s => s.Id == shiftId);
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(ShiftsLoggerRepository)}\n" +
                $"Method: {nameof(IsValidShiftId)}\n" +
                $"There was an unexpected error querying the validity of the shift id: {shiftId}: {ex.Message}";
            _logger.LogError(ex, "{msg}\n\n", _errorMessage);
            return false;
        }
    }

    public async Task<bool> IsValidShiftOwnership(UserParams userParams)
    {
        try
        {
            return await _context.Shifts
                .AsNoTracking()
                .AnyAsync(s => s.Id == userParams.ShiftId && s.UserId.Equals(userParams.UserId));
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(ShiftsLoggerRepository)}\n" +
                $"Method: {nameof(IsValidShiftOwnership)}\n" +
                $"There was an unexpected error querying the validity of the ownership of shift: {userParams.ShiftId}: {ex.Message}";
            _logger.LogError(ex, "{msg}\n\n", _errorMessage);
            return false;
        }
    }
}
