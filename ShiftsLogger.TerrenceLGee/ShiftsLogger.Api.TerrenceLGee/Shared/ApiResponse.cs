namespace ShiftsLogger.Api.TerrenceLGee.Shared;

public class ApiResponse<T>
{
    public int StatusCode { get; set; }
    public bool Success { get; set; }
    public T? Data { get; set; } = default;
    public List<string> Errors { get; set; } = [];

    public ApiResponse()
    {
    }

    public ApiResponse(int statusCode, T? data)
    {
        StatusCode = statusCode;
        Success = true;
        Data = data;
        Errors = [];
    }

    public ApiResponse(int statusCode, List<string> errors)
    {
        StatusCode = statusCode;
        Success = false;
        Errors = errors;
    }
}
