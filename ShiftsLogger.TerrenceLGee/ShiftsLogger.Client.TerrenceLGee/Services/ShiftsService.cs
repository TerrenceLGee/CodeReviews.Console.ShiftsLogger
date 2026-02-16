using Microsoft.Extensions.Logging;
using ShiftsLogger.Client.TerrenceLGee.Data;
using ShiftsLogger.Client.TerrenceLGee.Models;
using ShiftsLogger.Client.TerrenceLGee.Services.Interfaces;
using ShiftsLogger.Contracts.TerrenceLGee.DTOs.ShiftDTOs;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace ShiftsLogger.Client.TerrenceLGee.Services;

public class ShiftsService : IShiftsService
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly ILogger<ShiftsService> _logger;
    private string _errorMessage = string.Empty;
    private const string ClientName = "client";
    private const string Scheme = "Bearer";
    private const string LogErrorString = "{msg}\n\n";

    public ShiftsService(IHttpClientFactory clientFactory, ILogger<ShiftsService> logger)
    {
        _clientFactory = clientFactory;
        _logger = logger;
    }

    public async Task<ShiftData?> AddShiftAsync(CreateShiftDto shift, AuthData data)
    {
        try
        {
            var httpClient = _clientFactory.CreateClient(ClientName);
            var url = $"{Urls.BaseUrl}{Urls.AddShiftUrl}";

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Scheme, data.AccessToken);

            var content = new StringContent(JsonSerializer.Serialize(shift), Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync(url, content);

            if (!response.IsSuccessStatusCode)
            {
                return new ShiftData
                {
                    ErrorMessage = $"Unable to add new shift\nReason: {response.ReasonPhrase}"
                };
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var shiftResponse = JsonSerializer.Deserialize<ShiftRoot>(responseContent, new JsonSerializerOptions
            { PropertyNameCaseInsensitive = true });

            if (shiftResponse is null)
            {
                return new ShiftData
                {
                    ErrorMessage = $"Unable to add new shift"
                };
            }

            if (shiftResponse.Data is null)
            {
                return new ShiftData
                {
                    ErrorMessage = $"Unable to add new shift:\n{string.Join('\n', shiftResponse.Errors)}"
                };
            }

            return shiftResponse.Data;
        }
        catch (HttpRequestException ex)
        {
            _errorMessage = $"\nClass: {nameof(ShiftsService)}\n" +
                $"Method: {nameof(AddShiftAsync)}\n" +
                $"There was an API error attempting to add a new shift: {ex.Message}\n";
            _logger.LogError(ex, LogErrorString, _errorMessage);
            return new ShiftData
            {
                ErrorMessage = "Request to add new shift failed"
            };
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(ShiftsService)}\n" +
                $"Method: {nameof(AddShiftAsync)}\n" +
                $"There was an unexpected error attempting to add a new shift: {ex.Message}\n";
            _logger.LogError(ex, LogErrorString, _errorMessage);
            return new ShiftData
            {
                ErrorMessage = "Unexpected error. Failed to add new shift"
            };
        }
    }

    public async Task<ShiftData?> UpdateShiftAsync(UpdateShiftDto shift, AuthData data)
    {
        try
        {
            var httpClient = _clientFactory.CreateClient(ClientName);
            var url = $"{Urls.BaseUrl}{Urls.UpdateShiftUrl}{shift.Id}";

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Scheme, data.AccessToken);

            var content = new StringContent(JsonSerializer.Serialize(shift), Encoding.UTF8, "application/json");

            var response = await httpClient.PutAsync(url, content);

            if (!response.IsSuccessStatusCode)
            {
                return new ShiftData
                {
                    ErrorMessage = $"Unable to update shift {shift.Id}\nREason: {response.ReasonPhrase}"
                };
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var shiftResponse = JsonSerializer.Deserialize<ShiftRoot>(responseContent, new JsonSerializerOptions
            { PropertyNameCaseInsensitive = true });

            if (shiftResponse is null)
            {
                return new ShiftData
                {
                    ErrorMessage = $"Unable to update shift {shift.Id}"
                };
            }

            if (shiftResponse.Data is null)
            {
                return new ShiftData
                {
                    ErrorMessage = $"Unable to update shift {shift.Id}:\n{string.Join('\n', shiftResponse.Errors)}"
                };
            }

            return shiftResponse.Data;
        }
        catch (HttpRequestException ex)
        {
            _errorMessage = $"\nClass: {nameof(ShiftsService)}\n" +
                $"Method: {nameof(UpdateShiftAsync)}\n" +
                $"There was an API error attempting to update shift {shift.Id}: {ex.Message}\n";
            _logger.LogError(ex, LogErrorString, _errorMessage);
            return new ShiftData
            {
                ErrorMessage = $"Request to update shift {shift.Id} failed"
            };
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(ShiftsService)}\n" +
                $"Method: {nameof(UpdateShiftAsync)}\n" +
                $"There was an unexpected error attempting to update shift {shift.Id}: {ex.Message}\n";
            _logger.LogError(ex, LogErrorString, _errorMessage);
            return new ShiftData
            {
                ErrorMessage = $"Unexpected error occured. Failed to update shift {shift.Id}"
            };
        }
    }

    public async Task<string?> DeleteShiftAsync(int shiftId, AuthData data)
    {
        try
        {
            var errorMessageToReturn = string.Empty;

            var httpClient = _clientFactory.CreateClient(ClientName);
            var url = $"{Urls.BaseUrl}{Urls.DeleteShiftUrl}{shiftId}";

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Scheme, data.AccessToken);

            var response = await httpClient.DeleteAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                errorMessageToReturn = $"Unable to delete shift {shiftId}\nReason: {response.ReasonPhrase}";
                return errorMessageToReturn;
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var shiftResponse = JsonSerializer.Deserialize<DeletionRoot>(responseContent, new JsonSerializerOptions
            { PropertyNameCaseInsensitive = true });

            if (shiftResponse is null)
            {
                errorMessageToReturn = $"Unable to delete shift {shiftId}";
                return errorMessageToReturn;
            }

            if (!shiftResponse.Success)
            {
                errorMessageToReturn = $"Unable to delete shift {shiftId}: {shiftResponse.Data}\n";
                return errorMessageToReturn;
            }

            if (string.IsNullOrEmpty(shiftResponse.Data))
            {
                errorMessageToReturn = $"Unable to delete shift {shiftId}\n: {string.Join('\n', shiftResponse.Errors)}";
                return errorMessageToReturn;
            }

            return shiftResponse.Data;
        }
        catch (HttpRequestException ex)
        {
            _errorMessage = $"\nClass: {nameof(ShiftsService)}\n" +
                $"Method: {nameof(DeleteShiftAsync)}\n" +
                $"There was an API error attempting to delete shift {shiftId}: {ex.Message}\n";
            _logger.LogError(ex, LogErrorString, _errorMessage);
            return $"Request to delete shift {shiftId} failed";
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(ShiftsService)}\n" +
                $"Method: {nameof(DeleteShiftAsync)}\n" +
                $"There was an unexpected error attempting to delete shift {shiftId}: {ex.Message}\n";
            _logger.LogError(ex, LogErrorString, _errorMessage);
            return $"Error while connecting to endpoint unable to delete shift {shiftId}";
        }
    }

    public async Task<ShiftData?> GetShiftAsync(int shiftId, AuthData data)
    {
        try
        {
            var httpClient = _clientFactory.CreateClient(ClientName);
            var url = $"{Urls.BaseUrl}{Urls.GetShiftUrl}{shiftId}";

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Scheme, data.AccessToken);

            var response = await httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                return new ShiftData
                {
                    ErrorMessage = $"Unable to retrieve shift {shiftId}\nReason: {response.ReasonPhrase}"
                };
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var shiftResponse = JsonSerializer.Deserialize<ShiftRoot>(responseContent, new JsonSerializerOptions
            { PropertyNameCaseInsensitive = true });

            if (shiftResponse is null)
            {
                return new ShiftData
                {
                    ErrorMessage = $"Unable to retrieve shift {shiftId}"
                };
            }

            if (shiftResponse.Data is null)
            {
                return new ShiftData
                {
                    ErrorMessage = $"Unable to retrieve shift {shiftId}:\n{string.Join('\n', shiftResponse.Errors)}"
                };
            }

            return shiftResponse.Data;
        }
        catch (HttpRequestException ex)
        {
            _errorMessage = $"\nClass: {nameof(ShiftsService)}\n" +
                $"Method: {nameof(GetShiftAsync)}\n" +
                $"There was an API error attempting to retrieve shift {shiftId}: {ex.Message}\n";
            _logger.LogError(ex, LogErrorString, _errorMessage);
            return new ShiftData
            {
                ErrorMessage = $"Request to retrieve shift {shiftId} failed"
            };
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(ShiftsService)}\n" +
                $"Method: {nameof(GetShiftAsync)}\n" +
                $"There was an unexpected error attempting to retrieve shift: {ex.Message}\n";
            _logger.LogError(ex, LogErrorString, _errorMessage);
            return new ShiftData
            {
                ErrorMessage = $"Error while connecting to the endpoint unable to retrieve shift {shiftId}"
            };
        }
    }

    public async Task<List<ShiftsData>> GetShiftsAsync(PaginationParams paginationParams, AuthData data)
    {
        try
        {
            var pagination = $"?page={paginationParams.Page}&pageSize={paginationParams.PageSize}";
            var httpClient = _clientFactory.CreateClient(ClientName);
            var url = $"{Urls.BaseUrl}{Urls.GetShiftsUrl}{pagination}";

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Scheme, data.AccessToken);

            var response = await httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode) return [];

            var responseContent = await response.Content.ReadAsStringAsync();
            var shiftsResponse = JsonSerializer.Deserialize<ShiftsRoot>(responseContent, new JsonSerializerOptions
            { PropertyNameCaseInsensitive = true });

            if (shiftsResponse is null) return [];

            return shiftsResponse.Data;
        }
        catch (HttpRequestException ex)
        {
            _errorMessage = $"\nClass: {nameof(ShiftsService)}\n" +
                $"Method: {nameof(GetShiftsAsync)}\n" +
                $"There was an API error attempting to retrieve shifts: {ex.Message}\n";
            _logger.LogError(ex, LogErrorString, _errorMessage);
            return [];
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(ShiftsService)}\n" +
                $"Method: {nameof(GetShiftsAsync)}\n" +
                $"There was an unexpected error attempting to retrieve shifts: {ex.Message}\n";
            _logger.LogError(ex, LogErrorString, _errorMessage);
            return [];
        }
    }

    public async Task<List<ShiftsData>> GetAllShiftsForAdminAsync(PaginationParams paginationParams, AuthData data)
    {
        try
        {
            var pagination = $"?page={paginationParams.Page}&pageSize={paginationParams.PageSize}";
            var httpClient = _clientFactory.CreateClient(ClientName);
            var url = $"{Urls.BaseUrl}{Urls.GetAllShiftsForAdminUrl}{pagination}";

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Scheme, data.AccessToken);

            var response = await httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode) return [];

            var responseContent = await response.Content.ReadAsStringAsync();
            var shiftsResponse = JsonSerializer.Deserialize<ShiftsRoot>(responseContent, new JsonSerializerOptions
            { PropertyNameCaseInsensitive = true });

            if (shiftsResponse is null) return [];

            return shiftsResponse.Data;
        }
        catch (HttpRequestException ex)
        {
            _errorMessage = $"\nClass: {nameof(ShiftsService)}\n" +
                $"Method: {nameof(GetAllShiftsForAdminAsync)}\n" +
                $"There was an API error attempting to retrieve all shifts: {ex.Message}\n";
            _logger.LogError(ex, LogErrorString, _errorMessage);
            return [];
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(ShiftsService)}\n" +
                $"Method: {nameof(GetAllShiftsForAdminAsync)}\n" +
                $"There was an unexpected error attempting to retieve all shifts: {ex.Message}\n";
            _logger.LogError(ex, LogErrorString, _errorMessage);
            return [];
        }
    }

    public async Task<(int, string?)> GetShiftsCountAsync(AuthData data)
    {
        try
        {
            var httpClient = _clientFactory.CreateClient(ClientName);
            var url = $"{Urls.BaseUrl}{Urls.GetShiftsCountUrl}";

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Scheme, data.AccessToken);

            var response = await httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                return (-1, response.ReasonPhrase);
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var shiftsResponse = JsonSerializer.Deserialize<ShiftsCountRoot>(responseContent, new JsonSerializerOptions
            { PropertyNameCaseInsensitive = true });

            if (shiftsResponse is null)
            {
                return (-1, "Unable to retrieve shifts count");
            }

            return (shiftsResponse.Data, shiftsResponse.ErrorMessage);
        }
        catch (HttpRequestException ex)
        {
            _errorMessage = $"\nClass: {nameof(ShiftsService)}\n" +
                $"Method: {nameof(GetShiftsCountAsync)}\n" +
                $"There was an API error attempting to retrieve shifts count: {ex.Message}\n";
            _logger.LogError(ex, LogErrorString, _errorMessage);
            return (-1, "Request to retrieve shifts count failed");
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(ShiftsService)}\n" +
                $"Method: {nameof(GetShiftsCountAsync)}\n" +
                $"There was an unexpected error attempting to retrieve shifts count: {ex.Message}\n";
            _logger.LogError(ex, LogErrorString, _errorMessage);
            return (-1, "Error connecting to the endpoint unable to retrieve shifts count");
        }
    }

    public async Task<(int, string?)> GetAllShiftsCountForAdminAsync(AuthData data)
    {
        try
        {
            var httpClient = _clientFactory.CreateClient(ClientName);
            var url = $"{Urls.BaseUrl}{Urls.GetAllShiftsCountForAdminUrl}";

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Scheme, data.AccessToken);

            var response = await httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                return (-1, response.ReasonPhrase);
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var shiftsResponse = JsonSerializer.Deserialize<ShiftsCountRoot>(responseContent, new JsonSerializerOptions
            { PropertyNameCaseInsensitive = true });

            if (shiftsResponse is null)
            {
                return (-1, "Unable to retrieve count of all shifts");
            }

            return (shiftsResponse.Data, shiftsResponse.ErrorMessage);
        }
        catch (HttpRequestException ex)
        {
            _errorMessage = $"\nClass: {nameof(ShiftsService)}\n" +
                $"Method: {nameof(GetAllShiftsCountForAdminAsync)}\n" +
                $"There was an API error attempting to retrieve count of all shifts: {ex.Message}\n";
            _logger.LogError(ex, LogErrorString, _errorMessage);
            return (-1, "Request to retrieve count of all shifts failed");
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(ShiftsService)}\n" +
                $"Method: {nameof(GetAllShiftsCountForAdminAsync)}\n" +
                $"There was an unexpected error attempting to retrieve count of all shifts: {ex.Message}\n";
            _logger.LogError(ex, LogErrorString, _errorMessage);
            return (-1, "Error connecting to the endpoint unable to retrieve count of all shifts");
        }
    }
}
