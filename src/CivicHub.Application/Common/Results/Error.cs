namespace CivicHub.Application.Common.Results;

public record Error(string Message, ErrorType Type);