using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShiftsLogger.Api.TerrenceLGee.Data.DatabaseConfigs;
using ShiftsLogger.Api.TerrenceLGee.Models;

namespace ShiftsLogger.Api.TerrenceLGee.Data;

public class ShiftsLoggerDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
{
    public DbSet<Shift> Shifts { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    public ShiftsLoggerDbContext(DbContextOptions<ShiftsLoggerDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfiguration(new UserConfiguration());
    }
}
