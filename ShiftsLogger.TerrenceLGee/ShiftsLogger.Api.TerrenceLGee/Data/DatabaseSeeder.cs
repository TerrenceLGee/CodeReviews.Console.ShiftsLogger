using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShiftsLogger.Contracts.TerrenceLGee.Enums;
using ShiftsLogger.Api.TerrenceLGee.Models;

namespace ShiftsLogger.Api.TerrenceLGee.Data;

public static class DatabaseSeeder
{
    public static async Task SeedUsersAsync(
        ShiftsLoggerDbContext dbContext,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        if (await dbContext.Users.AnyAsync()) return;

        var adminRole = new IdentityRole { Name = "admin" };
        var employeeRole = new IdentityRole { Name = "employee" };

        await roleManager.CreateAsync(adminRole);
        await roleManager.CreateAsync(employeeRole);

        var admin = new ApplicationUser
        {
            FirstName = "John",
            LastName = "Smith",
            Department = Department.InformationTechnology,
            Email = "admin@example.com",
            UserName = "admin@example.com",
        };

        await userManager.CreateAsync(admin, "Pa$$w0rd");
        await userManager.AddToRoleAsync(admin, "admin");

        var user = new ApplicationUser
        {
            FirstName = "Gordon",
            LastName = "Ramsay",
            Department = Department.Development,
            Email = "gramsay@example.com",
            UserName = "gramsay@example.com"
        };

        await userManager.CreateAsync(user, "Pa$$w0rd");
        await userManager.AddToRoleAsync(user, "employee");

        var userId = user.Id;
        await SeedShiftsAsync(dbContext, userId);
    }

    private static async Task SeedShiftsAsync(ShiftsLoggerDbContext dbContext, string userId)
    {
        var shifts = new[]
        {
            new Shift
            {
                UserId = userId,
                ShiftStart = DateTime.Parse("2026-01-10 07:30"),
                ShiftEnd = DateTime.Parse("2026-01-10 15:30"),
                Duration = TimeSpan.FromHours(8),
            },
            new Shift
            {
                UserId = userId,
                ShiftStart = DateTime.Parse("2026-01-11 07:30"),
                ShiftEnd = DateTime.Parse("2026-01-11 15:30"),
                Duration = TimeSpan.FromHours(8),
            },
            new Shift
            {
                UserId = userId,
                ShiftStart = DateTime.Parse("2026-01-12 07:30"),
                ShiftEnd = DateTime.Parse("2026-01-12 15:30"),
                Duration = TimeSpan.FromHours(8),
            },
            new Shift
            {
                UserId = userId,
                ShiftStart = DateTime.Parse("2026-01-13 07:30"),
                ShiftEnd = DateTime.Parse("2026-01-13 15:30"),
                Duration = TimeSpan.FromHours(8),
            },
            new Shift
            {
                UserId = userId,
                ShiftStart = DateTime.Parse("2026-01-14 07:30"),
                ShiftEnd = DateTime.Parse("2026-01-14 15:30"),
                Duration = TimeSpan.FromHours(8),
            },
            new Shift
            {
                UserId = userId,
                ShiftStart = DateTime.Parse("2026-01-15 07:30"),
                ShiftEnd = DateTime.Parse("2026-01-15 15:30"),
                Duration = TimeSpan.FromHours(8),
            },
            new Shift
            {
                UserId = userId,
                ShiftStart = DateTime.Parse("2026-01-16 07:30"),
                ShiftEnd = DateTime.Parse("2026-01-16 15:30"),
                Duration = TimeSpan.FromHours(8),
            },
            new Shift
            {
                UserId = userId,
                ShiftStart = DateTime.Parse("2026-01-17 07:30"),
                ShiftEnd = DateTime.Parse("2026-01-17 15:30"),
                Duration = TimeSpan.FromHours(8),
            },
            new Shift
            {
                UserId = userId,
                ShiftStart = DateTime.Parse("2026-01-18 07:30"),
                ShiftEnd = DateTime.Parse("2026-01-18 15:30"),
                Duration = TimeSpan.FromHours(8),
            },
            new Shift
            {
                UserId = userId,
                ShiftStart = DateTime.Parse("2026-01-19 07:30"),
                ShiftEnd = DateTime.Parse("2026-01-19 15:30"),
                Duration = TimeSpan.FromHours(8),
            },
            new Shift
            {
                UserId = userId,
                ShiftStart = DateTime.Parse("2026-01-20 07:30"),
                ShiftEnd = DateTime.Parse("2026-01-20 15:30"),
                Duration = TimeSpan.FromHours(8),
            },
            new Shift
            {
                UserId = userId,
                ShiftStart = DateTime.Parse("2026-01-21 07:30"),
                ShiftEnd = DateTime.Parse("2026-01-21 15:30"),
                Duration = TimeSpan.FromHours(8),
            },
            new Shift
            {
                UserId = userId,
                ShiftStart = DateTime.Parse("2026-01-22 07:30"),
                ShiftEnd = DateTime.Parse("2026-01-22 15:30"),
                Duration = TimeSpan.FromHours(8),
            },
            new Shift
            {
                UserId = userId,
                ShiftStart = DateTime.Parse("2026-01-23 07:30"),
                ShiftEnd = DateTime.Parse("2026-01-23 15:30"),
                Duration = TimeSpan.FromHours(8),
            },
            new Shift
            {
                UserId = userId,
                ShiftStart = DateTime.Parse("2026-01-24 07:30"),
                ShiftEnd = DateTime.Parse("2026-01-24 15:30"),
                Duration = TimeSpan.FromHours(8),
            },
            new Shift
            {
                UserId = userId,
                ShiftStart = DateTime.Parse("2026-01-25 07:30"),
                ShiftEnd = DateTime.Parse("2026-01-25 15:30"),
                Duration = TimeSpan.FromHours(8),
            },
            new Shift
            {
                UserId = userId,
                ShiftStart = DateTime.Parse("2026-01-26 07:30"),
                ShiftEnd = DateTime.Parse("2026-01-26 15:30"),
                Duration = TimeSpan.FromHours(8),
            },
            new Shift
            {
                UserId = userId,
                ShiftStart = DateTime.Parse("2026-01-27 07:30"),
                ShiftEnd = DateTime.Parse("2026-01-27 15:30"),
                Duration = TimeSpan.FromHours(8),
            },
            new Shift
            {
                UserId = userId,
                ShiftStart = DateTime.Parse("2026-01-28 07:30"),
                ShiftEnd = DateTime.Parse("2026-01-28 15:30"),
                Duration = TimeSpan.FromHours(8),
            },
            new Shift
            {
                UserId = userId,
                ShiftStart = DateTime.Parse("2026-01-29 07:30"),
                ShiftEnd = DateTime.Parse("2026-01-29 15:30"),
                Duration = TimeSpan.FromHours(8),
            }
        };


        await dbContext.Shifts.AddRangeAsync(shifts);
        await dbContext.SaveChangesAsync();
    }

}
