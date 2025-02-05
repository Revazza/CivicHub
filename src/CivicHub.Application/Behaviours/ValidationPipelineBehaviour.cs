using CivicHub.Application.Common.Results;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CivicHub.Application.Behaviours;

public class ValidationPipelineBehaviour<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<ValidationPipelineBehaviour<TRequest, TResponse>> _logger;
    private readonly IValidator<TRequest>? _validator;

    public ValidationPipelineBehaviour(
        ILogger<ValidationPipelineBehaviour<TRequest, TResponse>> logger,
        IValidator<TRequest>? validator = null)
    {
        _logger = logger;
        _validator = validator;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (_validator is null)
        {
            return await next();
        }

        _logger.LogInformation("Starting validating request: {RequestName}", typeof(TRequest).Name);

        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (validationResult.IsValid)
        {
            _logger.LogInformation("Validation passed for request: {RequestName}", typeof(TRequest).Name);
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
        
        _logger.LogInformation("Validation failed for request: {RequestName}, Reason(s): {Reasons}",
            typeof(TRequest).Name, errorMessages);
    }
}