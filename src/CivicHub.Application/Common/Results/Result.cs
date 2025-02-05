using System.Text.Json.Serialization;
using CivicHub.Application.Common.Extensions;

namespace CivicHub.Application.Common.Results;

public class Result
{
    public bool IsSuccess => Errors.IsEmpty();
    
    public List<Error> Errors { get; }

    protected internal Result(List<Error> errors)
    {
        Errors = errors;
    }
    
    protected internal Result(Error error)
    {
        Errors = [error];
    }

    protected internal Result()
    {
        Errors = [];
    }

    public static Result Failure(Error error) => new(error);
    
    public static Result Success() => new();
    
    public static implicit operator Result(List<Error> errors) => new(errors);
    
    public static implicit operator Result(Error error) => new(error);
}