using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShiftsLogger.Api.TerrenceLGee.Models;
using ShiftsLogger.Api.TerrenceLGee.Services.Interfaces;
using ShiftsLogger.Api.TerrenceLGee.Shared;
using ShiftsLogger.Api.TerrenceLGee.Shared.Pagination;
using ShiftsLogger.Api.TerrenceLGee.Shared.Parameters;
using ShiftsLogger.Contracts.TerrenceLGee.DTOs.ShiftDTOs;
using System.Security.Claims;

namespace ShiftsLogger.Api.TerrenceLGee.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ShiftsController : ControllerBase
{
    private readonly IShiftsLoggerService _shiftsLoggerService;
    private readonly UserManager<ApplicationUser> _userManager;

    public ShiftsController(IShiftsLoggerService shiftsLoggerService, UserManager<ApplicationUser> userManager)
    {
        _shiftsLoggerService = shiftsLoggerService;
        _userManager = userManager;
    }

    [HttpPost("add")]
    [Authorize]
    public async Task<ActionResult<ApiResponse<RetrievedShiftDto?>>> AddShift([FromBody] CreateShiftDto shift)
    {
        ApiResponse<RetrievedShiftDto?> response;
        if (!ModelState.IsValid)
        {
            var modelErrors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
            response = new ApiResponse<RetrievedShiftDto?>(422, modelErrors);
            return StatusCode(response.StatusCode, response);
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId is null)
        {
            response = new ApiResponse<RetrievedShiftDto?>(404, ["User Id not found"]);
            return StatusCode(response.StatusCode, response);
        }

        var userState = await IsUserValidAndAuthorized(userId);

        if (userState != UserState.Authorized)
        {
            response = userState switch
            {
                UserState.NotFound => new ApiResponse<RetrievedShiftDto?>(404, ["User not found"]),
                UserState.Unauthorized => new ApiResponse<RetrievedShiftDto?>(401, ["Unauthorized"]),
                _ => new ApiResponse<RetrievedShiftDto?>(400, ["Bad Request"])
            };

            return StatusCode(response.StatusCode, response);
        }

        shift.UserId = userId;

        var result = await _shiftsLoggerService.AddShiftAsync(shift);

        if (result.IsFailure)
        {
            if (result.ErrorMessage!.Contains("Error adding new shift"))
            {
                response = new ApiResponse<RetrievedShiftDto?>(400, [result.ErrorMessage]);
            }
            else
            {
                response = new ApiResponse<RetrievedShiftDto?>(500, [result.ErrorMessage]);
            }

            return StatusCode(response.StatusCode, response);
        }

        response = new ApiResponse<RetrievedShiftDto?>(201, result.Value);

        return StatusCode(response.StatusCode, response);
    }

    [HttpPut("update/{shiftId:int}")]
    [Authorize]
    public async Task<ActionResult<ApiResponse<RetrievedShiftDto?>>> UpdateShifts([FromBody] UpdateShiftDto shift, [FromRoute] int shiftId)
    {
        ApiResponse<RetrievedShiftDto?> response;
        if (!ModelState.IsValid)
        {
            var modelErrors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
            response = new ApiResponse<RetrievedShiftDto?>(422, modelErrors);
            return StatusCode(response.StatusCode, response);
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId is null)
        {
            response = new ApiResponse<RetrievedShiftDto?>(404, ["User Id not found"]);
            return StatusCode(response.StatusCode, response);
        }

        var userState = await IsUserValidAndAuthorized(userId);

        if (userState != UserState.Authorized)
        {
            response = userState switch
            {
                UserState.NotFound => new ApiResponse<RetrievedShiftDto?>(404, ["User not found"]),
                UserState.Unauthorized => new ApiResponse<RetrievedShiftDto?>(401, ["Unauthorized"]),
                _ => new ApiResponse<RetrievedShiftDto?>(400, ["Bad Request"])
            };

            return StatusCode(response.StatusCode, response);
        }

        shift.Id = shiftId;
        shift.UserId = userId;

        var result = await _shiftsLoggerService.UpdateShiftAsync(shift);

        if (result.IsFailure)
        {
            if (result.ErrorMessage!.Contains("Invalid credentials"))
            {
                response = new ApiResponse<RetrievedShiftDto?>(401, [result.ErrorMessage]);
            }
            else if (result.ErrorMessage!.Contains("No shift with id"))
            {
                response = new ApiResponse<RetrievedShiftDto?>(404, [result.ErrorMessage]);
            }
            else if (result.ErrorMessage!.Contains("Error updating shift"))
            {
                response = new ApiResponse<RetrievedShiftDto?>(400, [result.ErrorMessage]);
            }
            else
            {
                response = new ApiResponse<RetrievedShiftDto?>(500, [result.ErrorMessage]);
            }

            return StatusCode(response.StatusCode, response);
        }

        response = new ApiResponse<RetrievedShiftDto?>(200, result.Value);

        return StatusCode(response.StatusCode, response);
    }

    [HttpDelete("delete/{shiftId:int}")]
    [Authorize]
    public async Task<ActionResult<ApiResponse<string?>>> DeleteShift([FromRoute] int shiftId)
    {
        ApiResponse<string?> response;

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId is null)
        {
            response = new ApiResponse<string?>(404, ["User Id not found"]);
            return StatusCode(response.StatusCode, response);
        }

        var userState = await IsUserValidAndAuthorized(userId);

        if (userState != UserState.Authorized)
        {
            response = userState switch
            {
                UserState.NotFound => new ApiResponse<string?>(404, ["User not found"]),
                UserState.Unauthorized => new ApiResponse<string?>(401, ["Unauthorized"]),
                _ => new ApiResponse<string?>(400, ["Bad Request"])
            };

            return StatusCode(response.StatusCode, response);
        }

        var result = await _shiftsLoggerService.DeleteShiftAsync(new UserParams { ShiftId = shiftId, UserId = userId });

        if (result.IsFailure)
        {
            if (result.ErrorMessage!.Contains("Invalid credentials"))
            {
                response = new ApiResponse<string?>(401, [result.ErrorMessage]);
            }
            else if (result.ErrorMessage!.Contains("No shift with id"))
            {
                response = new ApiResponse<string?>(404, [result.ErrorMessage]);
            }
            else if (result.ErrorMessage!.Contains("Error deleting shift"))
            {
                response = new ApiResponse<string?>(400, [result.ErrorMessage]);
            }
            else
            {
                response = new ApiResponse<string?>(500, [result.ErrorMessage]);
            }

            return StatusCode(response.StatusCode, response);
        }

        response = new ApiResponse<string?>(200, $"Shift {shiftId} deleted successfully");

        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("admin")]
    [Authorize(Roles = "admin")]
    public async Task<ActionResult<ApiResponse<PagedList<RetrievedShiftDto>>>> GetShiftsAdmin([FromQuery] int page, [FromQuery] int pageSize)
    {
        ApiResponse<PagedList<RetrievedShiftDto>> response;

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId is null)
        {
            response = new ApiResponse<PagedList<RetrievedShiftDto>>(404, ["User Id not found"]);
            return StatusCode(response.StatusCode, response);
        }

        var userState = await IsUserValidAndAuthorized(userId);

        if (userState != UserState.Authorized)
        {
            response = userState switch
            {
                UserState.NotFound => new ApiResponse<PagedList<RetrievedShiftDto>>(404, ["User not found"]),
                UserState.Unauthorized => new ApiResponse<PagedList<RetrievedShiftDto>>(401, ["Unauthorized"]),
                _ => new ApiResponse<PagedList<RetrievedShiftDto>>(400, ["Bad Request"])
            };

            return StatusCode(response.StatusCode, response);
        }

        var paginationParams = new PaginationParams { Page = page, PageSize = pageSize };

        var result = await _shiftsLoggerService.GetAllShiftsForAdminAsync(paginationParams);

        response = new ApiResponse<PagedList<RetrievedShiftDto>>(200, result.Value);

        return StatusCode(response.StatusCode, response);
    }

    [HttpGet]
    [Authorize(Roles = "employee, admin")]
    public async Task<ActionResult<ApiResponse<PagedList<RetrievedShiftDto>>>> GetShifts([FromQuery] int page, [FromQuery] int pageSize)
    {
        ApiResponse<PagedList<RetrievedShiftDto>> response;

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId is null)
        {
            response = new ApiResponse<PagedList<RetrievedShiftDto>>(404, ["User Id not found"]);
            return StatusCode(response.StatusCode, response);
        }

        var userState = await IsUserValidAndAuthorized(userId);

        if (userState != UserState.Authorized)
        {
            response = userState switch
            {
                UserState.NotFound => new ApiResponse<PagedList<RetrievedShiftDto>>(404, ["User not found"]),
                UserState.Unauthorized => new ApiResponse<PagedList<RetrievedShiftDto>>(401, ["Unauthorized"]),
                _ => new ApiResponse<PagedList<RetrievedShiftDto>>(400, ["Bad Request"])
            };

            return StatusCode(response.StatusCode, response);
        }

        var paginationParams = new PaginationParams { UserId = userId, Page = page, PageSize = pageSize };

        var result = await _shiftsLoggerService.GetShiftsAsync(paginationParams);

        response = new ApiResponse<PagedList<RetrievedShiftDto>>(200, result.Value);

        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("{shiftId:int}")]
    [Authorize]
    public async Task<ActionResult<ApiResponse<RetrievedShiftDto?>>> GetShift([FromRoute] int shiftId)
    {
        ApiResponse<RetrievedShiftDto?> response;

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId is null)
        {
            response = new ApiResponse<RetrievedShiftDto?>(404, ["User Id not found"]);
            return StatusCode(response.StatusCode, response);
        }

        var userState = await IsUserValidAndAuthorized(userId);

        if (userState != UserState.Authorized)
        {
            response = userState switch
            {
                UserState.NotFound => new ApiResponse<RetrievedShiftDto?>(404, ["User not found"]),
                UserState.Unauthorized => new ApiResponse<RetrievedShiftDto?>(401, ["Unauthorized"]),
                _ => new ApiResponse<RetrievedShiftDto?>(400, ["Bad Request"])
            };

            return StatusCode(response.StatusCode, response);
        }

        var userParams = new UserParams { ShiftId = shiftId, UserId = userId };

        var result = await _shiftsLoggerService.GetShiftAsync(userParams);

        if (result.IsFailure)
        {
            if (result.ErrorMessage!.Contains("Invalid credentials"))
            {
                response = new ApiResponse<RetrievedShiftDto?>(401, [result.ErrorMessage]);
            }
            else if (result.ErrorMessage!.Contains("No shift with id"))
            {
                response = new ApiResponse<RetrievedShiftDto?>(404, [result.ErrorMessage]);
            }
            else if (result.ErrorMessage!.Contains("Error retrieving shift"))
            {
                response = new ApiResponse<RetrievedShiftDto?>(400, [result.ErrorMessage]);
            }
            else
            {
                response = new ApiResponse<RetrievedShiftDto?>(500, [result.ErrorMessage]);
            }

            return StatusCode(response.StatusCode, response);
        }

        response = new ApiResponse<RetrievedShiftDto?>(200, result.Value);

        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("count")]
    [Authorize]
    public async Task<ActionResult<ApiResponse<int>>> GetShiftsCount()
    {
        ApiResponse<int> response;

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId is null)
        {
            response = new ApiResponse<int>(404, ["User Id not found"]);
            return StatusCode(response.StatusCode, response);
        }

        var userState = await IsUserValidAndAuthorized(userId);

        if (userState != UserState.Authorized)
        {
            response = userState switch
            {
                UserState.NotFound => new ApiResponse<int>(404, ["User not found"]),
                UserState.Unauthorized => new ApiResponse<int>(401, ["Unauthorized"]),
                _ => new ApiResponse<int>(400, ["Bad Request"])
            };

            return StatusCode(response.StatusCode, response);
        }

        var userParams = new UserParams { UserId = userId };

        var result = await _shiftsLoggerService.GetCountOfShiftsAsync(userParams);

        if (result.IsFailure)
        {
            if (result.ErrorMessage!.Contains("Error retrieving the count"))
            {
                response = new ApiResponse<int>(400, [result.ErrorMessage]);
            }
            else
            {
                response = new ApiResponse<int>(500, [result.ErrorMessage]);
            }

            return StatusCode(response.StatusCode, response);
        }

        response = new ApiResponse<int>(200, result.Value);

        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("admin/count")]
    [Authorize(Roles = "admin")]
    public async Task<ActionResult<ApiResponse<int>>> GetAllShiftsCountForAdmin()
    {
        ApiResponse<int> response;

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId is null)
        {
            response = new ApiResponse<int>(404, ["User Id not found"]);
            return StatusCode(response.StatusCode, response);
        }

        var userState = await IsUserValidAndAuthorized(userId);

        if (userState != UserState.Authorized)
        {
            response = userState switch
            {
                UserState.NotFound => new ApiResponse<int>(404, ["User not found"]),
                UserState.Unauthorized => new ApiResponse<int>(401, ["Unauthorized"]),
                _ => new ApiResponse<int>(400, ["Bad Request"])
            };

            return StatusCode(response.StatusCode, response);
        }

        var result = await _shiftsLoggerService.GetCountOfAllShiftsForAdminAsync();

        if (result.IsFailure)
        {
            if (result.ErrorMessage!.Contains("Error retrieving the count"))
            {
                response = new ApiResponse<int>(400, [result.ErrorMessage]);
            }
            else
            {
                response = new ApiResponse<int>(500, [result.ErrorMessage]);
            }

            return StatusCode(response.StatusCode, response);
        }

        response = new ApiResponse<int>(200, result.Value);

        return StatusCode(response.StatusCode, response);
    }

    private async Task<UserState> IsUserValidAndAuthorized(string? userId)
    {
        var user = await _userManager.Users
            .Where(u => u.Id.Equals(userId))
            .Include(u => u.RefreshTokens)
            .FirstOrDefaultAsync();

        if (user is null) return UserState.NotFound;

        var refreshToken = user.RefreshTokens
            .LastOrDefault();

        if (refreshToken is null) return UserState.Unauthorized;

        if (refreshToken.IsRevoked) return UserState.Unauthorized;

        return UserState.Authorized;
    }

}


public enum UserState
{
    Authorized,
    Unauthorized,
    NotFound,
}