using CivicHub.Application.Common.Results;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CivicHub.Application.Behaviours;

public class ValidationPipelineBehaviour<TRequest, TResponse>(
    ILogger<ValidationPipelineBehaviour<TRequest, TResponse>> logger,
    IValidator<TRequest> validator = null)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (validator is null)
        {
            return await next();
        }

        logger.LogInformation("Starting validating request: {RequestName}", typeof(TRequest).Name);

        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (validationResult.IsValid)
        {
            logger.LogInformation("Validation passed for request: {RequestName}", typeof(TRequest).Name);
            return await next();
        }

        var errors = validationResult.Errors
            .GroupBy(validationFailure => validationFailure.PropertyName)
            .SelectMany(group =>
                group.Select(validationFailure => new Error(validationFailure.ErrorMessage, ErrorType.Validation)))
            .ToList();

        LogValidationFailures(errors);

        return (dynamic)errors;
    }

    private void LogValidationFailures(IEnumerable<Error> errors)
    {
        const string separator = " | ";
        var errorMessages = string.Join(separator, errors.Select(x => x.Message));

        logger.LogInformation("Validation failed for request: {RequestName}, Reason(s): {Reasons}",
            typeof(TRequest).Name, errorMessages);
    }
}