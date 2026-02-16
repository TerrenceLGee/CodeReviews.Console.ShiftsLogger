using ShiftsLogger.Api.TerrenceLGee.Models;
using ShiftsLogger.Contracts.TerrenceLGee.DTOs.ShiftDTOs;

namespace ShiftsLogger.Api.TerrenceLGee.Mappings.ShiftMappings;

public static class FromDto
{
    extension(CreateShiftDto dto)
    {
        public Shift FromCreateShiftDto()
        {
            return new()
            {
                UserId = dto.UserId,
                ShiftStart = dto.ShiftStart,
                ShiftEnd = dto.ShiftEnd,
                Duration = (dto.ShiftEnd.HasValue)
                ? dto.ShiftEnd.Value - dto.ShiftStart
                : null
            };
        }
    }

    extension(UpdateShiftDto dto)
    {
        public Shift FromUpdateShiftDto()
        {
            return new()
            {
                Id = dto.Id,
                UserId = dto.UserId,
                ShiftStart = dto.ShiftStart,
                ShiftEnd = dto.ShiftEnd,
                Duration = (dto.ShiftEnd.HasValue)
                ? dto.ShiftEnd - dto.ShiftStart
                : null
            };
        }
    }
}
