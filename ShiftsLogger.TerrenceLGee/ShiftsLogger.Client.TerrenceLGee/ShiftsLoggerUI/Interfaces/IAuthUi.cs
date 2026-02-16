using ShiftsLogger.Client.TerrenceLGee.Models;

namespace ShiftsLogger.Client.TerrenceLGee.ShiftsLoggerUI.Interfaces;

public interface IAuthUi
{
    public Task RegisterUserAsync();
    public Task<AuthData?> LoginAsync();
    public Task LogoutAsync(AuthData authData);
}
