using ShiftsLogger.Api.TerrenceLGee.Models;
using ShiftsLogger.Api.TerrenceLGee.Shared.Pagination;
using ShiftsLogger.Contracts.TerrenceLGee.DTOs.ShiftDTOs;
using ShiftsLogger.Api.TerrenceLGee.Shared.Extensions;
using ShiftsLogger.Api.TerrenceLGee.Shared.Parameters;

namespace ShiftsLogger.Api.TerrenceLGee.Mappings.ShiftMappings;

public static class ToDto
{
    extension(Shift shift)
    {
        public RetrievedShiftDto ToRetrievedShiftDto()
        {
            return new()
            {
                Id = shift.Id,
                UserId = shift.UserId,
                ShiftStart = shift.ShiftStart,
                ShiftEnd = shift.ShiftEnd,
                Duration = (shift.ShiftEnd.HasValue) 
                ? shift.ShiftEnd.Value - shift.ShiftStart 
                : null
            };
        }
    }

    extension(PagedList<Shift> shifts)
    {
        public PagedList<RetrievedShiftDto> ToPagedListOfRetrievedShiftDto(PaginationParams paginationParams)
        {
            return shifts
                .Select(s => s.ToRetrievedShiftDto()).ToPagedList(shifts.Count, paginationParams.Page, paginationParams.PageSize);
                
        }
    }
}
