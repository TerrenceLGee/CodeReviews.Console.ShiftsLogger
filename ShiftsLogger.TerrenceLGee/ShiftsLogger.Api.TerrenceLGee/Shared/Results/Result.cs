namespace ShiftsLogger.Api.TerrenceLGee.Shared.Results;

public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public string? ErrorMessage { get; }

    protected Result(bool isSuccess, string? errorMessage)
    {
        if (isSuccess && errorMessage is not null)
            throw new InvalidOperationException("Success result cannot have an error message");
        if (!isSuccess && errorMessage is null)
            throw new InvalidOperationException("Failure result must haven an error message");

        IsSuccess = isSuccess;
        ErrorMessage = errorMessage;
    }

    public static Result Ok() => new(true, null);
    public static Result Fail(string message) => new(false, message);
}

public class Result<T> : Result
{
    public T? Value { get; }

    protected internal Result(T? value)
        : base(isSuccess: true, errorMessage: null)
    {
        Value = value ?? throw new ArgumentNullException(nameof(value));
    }

    protected internal Result(string errorMessage)
        : base(isSuccess: false, errorMessage: errorMessage)
    {
        Value = default;
    }

    public static Result<T> Ok(T value) => new(value);
    public new static Result<T> Fail(string message) => new(message);
}
