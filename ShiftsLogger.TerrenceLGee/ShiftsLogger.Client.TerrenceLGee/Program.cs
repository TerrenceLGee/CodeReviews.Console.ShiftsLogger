using Microsoft.Extensions.DependencyInjection;
using Serilog;
using ShiftsLogger.Client.TerrenceLGee.Data;
using ShiftsLogger.Client.TerrenceLGee.Services;
using ShiftsLogger.Client.TerrenceLGee.Services.Auth;
using ShiftsLogger.Client.TerrenceLGee.Services.Interfaces;
using ShiftsLogger.Client.TerrenceLGee.ShiftsLoggerUI;
using ShiftsLogger.Client.TerrenceLGee.ShiftsLoggerUI.Helpers;
using ShiftsLogger.Client.TerrenceLGee.ShiftsLoggerUI.Interfaces;

try
{
    LoggingSetup();
    await Startup();
}
catch (Exception ex)
{
    UiHelpers.PressAnyKeyToContinueError($"An unexpected error occurred: {ex.Message}");
}

return;

async Task Startup()
{
    var services = new ServiceCollection()
        .AddLogging(loggingBuilder =>
        {
            loggingBuilder.AddSerilog(dispose: true);
        })
        .AddScoped<IAuthService, AuthService>()
        .AddScoped<IShiftsService, ShiftsService>()
        .AddScoped<IAuthUi, AuthUi>()
        .AddScoped<IShiftsUi, ShiftsUi>();

    services.AddHttpClient("client", c =>
    {
        c.BaseAddress = new Uri(Urls.BaseUrl);
    });

    var serviceProvider = services.BuildServiceProvider();

    var authUi = serviceProvider.GetRequiredService<IAuthUi>();
    var shiftsUi = serviceProvider.GetRequiredService<IShiftsUi>();

    var app = new ShiftsLoggerApp(authUi, shiftsUi);

    await app.RunAsync();
}

void LoggingSetup()
{
    var loggingDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Logs");
    Directory.CreateDirectory(loggingDirectory);
    var filePath = Path.Combine(loggingDirectory, "app-.txt");
    var outputTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}";

    Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Error()
        .WriteTo.File(
        path: filePath,
        rollingInterval: RollingInterval.Day,
        outputTemplate: outputTemplate)
        .CreateLogger();
}
