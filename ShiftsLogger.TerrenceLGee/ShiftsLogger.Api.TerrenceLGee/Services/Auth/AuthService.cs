using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ShiftsLogger.Api.TerrenceLGee.Data;
using ShiftsLogger.Api.TerrenceLGee.Data.Configuration;
using ShiftsLogger.Api.TerrenceLGee.Models;
using ShiftsLogger.Api.TerrenceLGee.Services.Interfaces.Auth;
using ShiftsLogger.Api.TerrenceLGee.Shared.Results;
using ShiftsLogger.Contracts.TerrenceLGee.DTOs.AuthDTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ShiftsLogger.Api.TerrenceLGee.Services.Auth;

public class AuthService : IAuthService
{
    private readonly IOptions<AuthConfiguration> _authOptions;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ShiftsLoggerDbContext _context;
    private readonly AuthConfiguration _authConfiguration;
    private readonly ILogger<AuthService> _logger;
    private string _errorMessage = string.Empty;

    public AuthService(
        IOptions<AuthConfiguration> authOptions,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        SignInManager<ApplicationUser> signInManager,
        ShiftsLoggerDbContext context,
        ILogger<AuthService> logger)
    {
        _authOptions = authOptions;
        _authConfiguration = _authOptions.Value;
        _userManager = userManager;
        _roleManager = roleManager;
        _signInManager = signInManager;
        _context = context;
        _logger = logger;
    }

    public async Task<Result> RegisterUserAsync(UserRegistrationDto userDto)
    {
        try
        {
            var existingUser = await _userManager
                .FindByEmailAsync(userDto.Email);

            if (existingUser is not null)
            {
                return Result.Fail("User already exists. Unable to register a previously registered user.");
            }

            var user = new ApplicationUser
            {
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                Email = userDto.Email,
                UserName = userDto.Email,
                Department = userDto.Department
            };

            var result = await _userManager.CreateAsync(user, userDto.Password);

            if (!result.Succeeded)
            {
                return Result.Fail("Unable to register user at this time. Please check your input and try again later.");
            }

            return Result.Ok();
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(AuthService)}\n" +
                $"Method: {RegisterUserAsync}\n" +
                $"An unexpected error occurred while registering the new user: {ex.Message}";
            _logger.LogError(ex, "{msg}\n\n", _errorMessage);
            return Result.Fail($"An unexpected error occurred while registering the new user");
        }
    }

    public async Task<Result<AuthenticationResponseDto?>> LoginUserAsync(UserLoginDto userDto)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(userDto.Email);

            if (user is null)
            {
                return Result<AuthenticationResponseDto?>.Fail("User not found");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, userDto.Password, false);

            if (!result.Succeeded)
            {
                return Result<AuthenticationResponseDto?>.Fail("Login failed");
            }

            var roles = await _userManager.GetRolesAsync(user);
            var userRole = roles.FirstOrDefault() ?? "employee";

            var role = await _roleManager.FindByNameAsync(userRole);
            var roleClaims = (role is not null)
                ? await _roleManager.GetClaimsAsync(role)
                : [];

            var jwtId = string.Empty;

            var jwtToken = GenerateJwtToken(user, userRole, roleClaims, out jwtId);
            var refreshToken = GenerateRefreshToken(jwtId, user.Id);

            await _context.RefreshTokens.AddAsync(refreshToken);
            await _context.SaveChangesAsync();

            var response = new AuthenticationResponseDto
            {
                AccessToken = jwtToken,
                RefreshToken = refreshToken.Token
            };

            return Result<AuthenticationResponseDto?>.Ok(response);
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(AuthService)}\n" +
                $"Method: {LoginUserAsync}\n" +
                $"An unexpected error occurred during user login: {ex.Message}";
            _logger.LogError(ex, "{msg}\n\n", _errorMessage);
            return Result<AuthenticationResponseDto?>.Fail("An unexpected error occurred during user login");
        }
    }

    public async Task<Result> LogoutUserAsync(UserLogoutDto userDto)
    {
        try
        {
            var refreshToken = await _context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.UserId.Equals(userDto.UserId) && !rt.IsRevoked);

            if (refreshToken is null)
            {
                return Result.Fail("Unable to proceed with logout. Invalid authorization");
            }

            refreshToken.IsRevoked = true;
            refreshToken.RevokedAt = DateTime.Now;
            await _context.SaveChangesAsync();

            return Result.Ok();
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(AuthService)}\n" +
                $"Method: {LogoutUserAsync}\n" +
                $"An unexpected error occurred during user logout: {ex.Message}";
            _logger.LogError(ex, "{msg}\n\n", _errorMessage);
            return Result.Fail("Unexpected error occurred during user logout.");
        }
    }

    private string GenerateJwtToken(
        ApplicationUser user, 
        string userRole, 
        IList<Claim> roleClaims, 
        out string jwtId)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authConfiguration.Key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        jwtId = Guid.NewGuid().ToString();

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Jti, jwtId),
            new Claim(JwtRegisteredClaimNames.Email, user.Email!),
            new Claim(JwtRegisteredClaimNames.Iss, _authConfiguration.Issuer),
            new Claim(JwtRegisteredClaimNames.Aud, _authConfiguration.Audience),
            new Claim("role", userRole)
        };

        foreach (var roleClaim in roleClaims)
        {
            claims.Add(new Claim(roleClaim.Type, roleClaim.Value));
        }

        var token = new JwtSecurityToken(
            issuer: _authConfiguration.Issuer,
            audience: _authConfiguration.Audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private RefreshToken GenerateRefreshToken(string jwtId, string userId)
    {
        var randomBytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);

        var refreshTokenExpirationDays = _authConfiguration.RefreshTokenExpirationDays;

        return new RefreshToken
        {
            Token = Convert.ToBase64String(randomBytes),
            JwtId = jwtId,
            Expires = DateTime.Now.AddDays(refreshTokenExpirationDays),
            UserId = userId,
            CreatedAt = DateTime.Now,
            IsRevoked = false,
            RevokedAt = null
        };
    }
}
