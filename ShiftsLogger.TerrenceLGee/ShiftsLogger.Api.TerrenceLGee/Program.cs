using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using ShiftsLogger.Api.TerrenceLGee.Data;
using ShiftsLogger.Api.TerrenceLGee.Data.Configuration;
using ShiftsLogger.Api.TerrenceLGee.Models;
using ShiftsLogger.Api.TerrenceLGee.Repositories;
using ShiftsLogger.Api.TerrenceLGee.Repositories.Interfaces;
using ShiftsLogger.Api.TerrenceLGee.Services;
using ShiftsLogger.Api.TerrenceLGee.Services.Auth;
using ShiftsLogger.Api.TerrenceLGee.Services.Interfaces;
using ShiftsLogger.Api.TerrenceLGee.Services.Interfaces.Auth;
using System.Text;

var loggingDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Logs");
Directory.CreateDirectory(loggingDirectory);
var filePath = Path.Combine(loggingDirectory, "app-.txt");
var outputTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}";

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File(filePath, rollingInterval: RollingInterval.Day, outputTemplate: outputTemplate)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

builder.Services.AddControllers();

builder.Services.AddOpenApi();

builder.Services.AddDbContext<ShiftsLoggerDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
    .ConfigureWarnings(warnings =>
    {
        warnings.Ignore(RelationalEventId.PendingModelChangesWarning);
    });
});

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    options.User.RequireUniqueEmail = true;
})
    .AddEntityFrameworkStores<ShiftsLoggerDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

var authConfiguration = new AuthConfiguration();
builder.Configuration.GetSection(authConfiguration.Section)
    .Bind(authConfiguration);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = authConfiguration.Issuer,
            ValidAudience = authConfiguration.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authConfiguration.Key))
        };
    });

builder.Services.Configure<AuthConfiguration>(builder.Configuration.GetSection(authConfiguration.Section));
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IShiftsLoggerRepository, ShiftsLoggerRepository>();
builder.Services.AddScoped<IShiftsLoggerService, ShiftsLoggerService>();

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ShiftsLoggerDbContext>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    await dbContext.Database.EnsureCreatedAsync();

    await DatabaseSeeder.SeedUsersAsync(dbContext, userManager, roleManager);
}

app.Run();
