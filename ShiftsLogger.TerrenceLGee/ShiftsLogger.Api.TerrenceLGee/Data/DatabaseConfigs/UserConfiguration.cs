using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShiftsLogger.Api.TerrenceLGee.Models;

namespace ShiftsLogger.Api.TerrenceLGee.Data.DatabaseConfigs;

public class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.HasMany<Shift>(u => u.Shifts)
            .WithOne(s => s.User)
            .HasForeignKey(s => s.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany<RefreshToken>(u => u.RefreshTokens)
            .WithOne(rt => rt.User)
            .HasForeignKey(rt => rt.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
