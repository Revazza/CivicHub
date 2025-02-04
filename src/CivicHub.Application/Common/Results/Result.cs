namespace CivicHub.Application.Common.Results;

public class Result
{
    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;
    
    protected internal Result(string errorMessage)
    {
        IsSuccess = false;
        ErrorMessage = errorMessage;
    }

    protected internal Result()
    {
        IsSuccess = true;
        ErrorMessage = string.Empty;
    }

    public string ErrorMessage { get; }

    public static Result Success() => new();

    public static Result<TValue> Success<TValue>(TValue value) => new(value);

    public static Result<TValue> Failure<TValue>(string errorMessage) => new(errorMessage);

    public static implicit operator Result(string errorMessage) => new(errorMessage);

}