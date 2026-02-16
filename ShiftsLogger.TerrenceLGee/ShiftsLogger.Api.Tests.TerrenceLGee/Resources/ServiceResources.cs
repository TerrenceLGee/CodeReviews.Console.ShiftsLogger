using ShiftsLogger.Api.TerrenceLGee.Shared.Pagination;
using ShiftsLogger.Api.TerrenceLGee.Shared.Parameters;
using ShiftsLogger.Api.TerrenceLGee.Shared.Results;
using ShiftsLogger.Contracts.TerrenceLGee.DTOs.ShiftDTOs;

namespace ShiftsLogger.Api.Tests.TerrenceLGee.Resources;

public static class ServiceResources
{
    public static readonly string UserId = "2eaf6d3e-a124-40c0-ab8d-d2d0d0231ed3";
    public static readonly int ShiftId = 1;
    public static readonly int ShiftCount = 9;

    public static Result<RetrievedShiftDto?> GetResultFromShiftAddSuccess()
    {
        return Result<RetrievedShiftDto?>.Ok(GetRetrievedShiftDtoAfterAdd());
    }

    public static Result<RetrievedShiftDto?> GetResultFromShiftAddFailure()
    {
        return Result<RetrievedShiftDto?>.Fail("Error adding new shift");
    }

    public static Result<RetrievedShiftDto?> GetResultFromShiftUpdateSuccess()
    {
        return Result<RetrievedShiftDto?>.Ok(GetRetrievedShiftDtoAfterUpdate());
    }

    public static Result<RetrievedShiftDto?> GetResultFromShiftUpdateFailureWhenShiftNotFound()
    {
        return Result<RetrievedShiftDto?>.Fail($"No shift with id {ShiftId} found");
    }

    public static Result<RetrievedShiftDto?> GetResultFromUpdateShiftFailureWhenShiftDoesNotBelongToUser()
    {
        return Result<RetrievedShiftDto?>.Fail($"Invalid credentials for accessing or modifying shift {ShiftId}");
    }

    public static Result<RetrievedShiftDto?> GetResultFromShiftUpdateFailureShiftIsNull()
    {
        return Result<RetrievedShiftDto?>.Fail($"Error updating shift {ShiftId}");
    }

    public static Result GetResultFromShiftDeletionSuccess()
    {
        return Result.Ok();
    }

    public static Result GetResultFromShiftDeleteFailureWhenShiftNotFound()
    {
        return Result.Fail($"No shift with id {ShiftId} found");
    }

    public static Result GetResultFromDeleteShiftFailureWhenShiftDoesNotBelongToUser()
    {
        return Result.Fail($"Invalid credentials for accessing or modifying shift {ShiftId}");
    }

    public static Result GetResultFromShiftDeletionFailure()
    {
        return Result.Fail($"Error deleting shift {ShiftId}");
    }

    public static Result<RetrievedShiftDto?> GetResultFromGetShiftSuccess()
    {
        return Result<RetrievedShiftDto?>.Ok(GetRetrievedShiftDto());
    }

    public static Result<RetrievedShiftDto?> GetResultFromGetShiftFailureWhenShiftNotFound()
    {
        return Result<RetrievedShiftDto?>.Fail($"No shift with id {ShiftId} found");
    }

    public static Result<RetrievedShiftDto?> GetResultFromGetShiftFailureWhenShiftDoesNotBelongToUser()
    {
        return Result<RetrievedShiftDto?>.Fail($"Invalid credentials for accessing or modifying shift {ShiftId}");
    }

    public static Result<RetrievedShiftDto?> GetResultFromGetShiftFailure()
    {
        return Result<RetrievedShiftDto?>.Fail($"Error retrieving shift {ShiftId}");
    }

    public static Result<PagedList<RetrievedShiftDto>> GetResultFromGetShifts()
    {
        return Result<PagedList<RetrievedShiftDto>>.Ok(GetPagedListOfRetrievedShiftDto(GetPaginationParams()));
    }

    public static Result<PagedList<RetrievedShiftDto>> GetResultFromGetShiftsWhenRepoCatchesException()
    {
        return Result<PagedList<RetrievedShiftDto>>.Ok([]);
    }

    public static Result<PagedList<RetrievedShiftDto>> GetResultFromGetShiftsForAdmin()
    {
        return Result<PagedList<RetrievedShiftDto>>.Ok(GetPagedListOfRetrievedShiftDto(GetPaginationParams()));
    }

    public static Result<PagedList<RetrievedShiftDto>> GetResultFromGetShiftsForAdminWhenRepoCatchesException()
    {
        return Result<PagedList<RetrievedShiftDto>>.Ok([]);
    }

    public static Result<int> GetResultFromGetShiftCountWhenThereAreShifts()
    {
        return Result<int>.Ok(ShiftCount);
    }

    public static Result<int> GetResultFromGetShiftCountWhenRepoCatchesException()
    {
        return Result<int>.Fail("Error retrieving the count of shifts");
    }

    public static Result<int> GetResultFromGetShiftCountForAdminWhenThereAreShifts()
    {
        return Result<int>.Ok(ShiftCount);
    }

    public static Result<int> GetResultFromGetShiftCountForAdminWhenRepoCatchesException()
    {
        return Result<int>.Fail("Error retrieving the count of all shifts");
    }

    public static CreateShiftDto GetCreateShiftDtoBeforeAdd()
    {
        return new()
        {
            UserId = UserId,
            ShiftStart = DateTime.Parse("02-14-2026 13:30"),
            ShiftEnd = DateTime.Parse("02-14-2026 21:30"),
            Duration = TimeSpan.FromHours(8)
        };
    }

    public static UpdateShiftDto GetUpdateShiftDtoBeforeUpdate()
    {
        return new()
        {
            Id = ShiftId,
            UserId = UserId,
            ShiftStart = DateTime.Parse("02-14-2026 13:30"),
            ShiftEnd = DateTime.Parse("02-14-2026 23:30"),
            Duration = TimeSpan.FromHours(10)
        };
    }

    public static RetrievedShiftDto? GetRetrievedShiftDtoAfterAdd()
    {
        return new()
        {
            Id = ShiftId,
            UserId = UserId,
            ShiftStart = DateTime.Parse("02-14-2026 13:30"),
            ShiftEnd = DateTime.Parse("02-14-2026 21:30"),
            Duration = TimeSpan.FromHours(8)
        };
    }

    public static RetrievedShiftDto? GetRetrievedShiftDtoAfterUpdate()
    {
        return new()
        {
            Id = ShiftId,
            UserId = UserId,
            ShiftStart = DateTime.Parse("02-14-2026 13:30"),
            ShiftEnd = DateTime.Parse("02-14-2026 23:30"),
            Duration = TimeSpan.FromHours(10)
        };
    }

    public static RetrievedShiftDto? GetRetrievedShiftDto()
    {
        return new()
        {
            Id = ShiftId,
            UserId = UserId,
            ShiftStart = DateTime.Parse("02-14-2026 13:30"),
            ShiftEnd = DateTime.Parse("02-14-2026 21:30"),
            Duration = TimeSpan.FromHours(8)
        };
    }

    public static PagedList<RetrievedShiftDto> GetPagedListOfRetrievedShiftDto(PaginationParams paginationParams)
    {
        var shiftId = ShiftId;

        var shifts = new List<RetrievedShiftDto>
        {
            new()
            {
                Id = shiftId++,
                UserId = UserId,
                ShiftStart = DateTime.Parse("02-14-2026 13:30"),
                ShiftEnd = DateTime.Parse("02-14-2026 21:30"),
                Duration = TimeSpan.FromHours(8)
            },
            new()
            {
                Id = shiftId++,
                UserId = UserId,
                ShiftStart = DateTime.Parse("02-14-2026 13:30"),
                ShiftEnd = DateTime.Parse("02-14-2026 21:30"),
                Duration = TimeSpan.FromHours(8)
            },
            new()
            {
                Id = shiftId++,
                UserId = UserId,
                ShiftStart = DateTime.Parse("02-14-2026 13:30"),
                ShiftEnd = DateTime.Parse("02-14-2026 21:30"),
                Duration = TimeSpan.FromHours(8)
            },
            new()
            {
                Id = shiftId++,
                UserId = UserId,
                ShiftStart = DateTime.Parse("02-14-2026 13:30"),
                ShiftEnd = DateTime.Parse("02-14-2026 21:30"),
                Duration = TimeSpan.FromHours(8)
            },
            new()
            {
                Id = shiftId++,
                UserId = UserId,
                ShiftStart = DateTime.Parse("02-14-2026 13:30"),
                ShiftEnd = DateTime.Parse("02-14-2026 21:30"),
                Duration = TimeSpan.FromHours(8)
            },
            new()
            {
                Id = shiftId++,
                UserId = UserId,
                ShiftStart = DateTime.Parse("02-14-2026 13:30"),
                ShiftEnd = DateTime.Parse("02-14-2026 21:30"),
                Duration = TimeSpan.FromHours(8)
            },
            new()
            {
                Id = shiftId++,
                UserId = UserId,
                ShiftStart = DateTime.Parse("02-14-2026 13:30"),
                ShiftEnd = DateTime.Parse("02-14-2026 21:30"),
                Duration = TimeSpan.FromHours(8)
            },
            new()
            {
                Id = shiftId++,
                UserId = UserId,
                ShiftStart = DateTime.Parse("02-14-2026 13:30"),
                ShiftEnd = DateTime.Parse("02-14-2026 21:30"),
                Duration = TimeSpan.FromHours(8)
            },
            new()
            {
                Id = shiftId++,
                UserId = UserId,
                ShiftStart = DateTime.Parse("02-14-2026 13:30"),
                ShiftEnd = DateTime.Parse("02-14-2026 21:30"),
                Duration = TimeSpan.FromHours(8)
            }
        };

        var count = shifts.Count;

        return new PagedList<RetrievedShiftDto>(shifts, count, paginationParams.Page, paginationParams.PageSize);
    }

    public static UserParams GetUserParams()
    {
        return new()
        {
            ShiftId = ShiftId,
            UserId = UserId
        };
    }

    public static PaginationParams GetPaginationParams()
    {
        return new()
        {
            ShiftId = ShiftId,
            UserId = UserId,
            Page = 1,
            PageSize = 3
        };
    }
}
