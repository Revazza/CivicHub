using CivicHub.Application.Common.Results;
using CivicHub.Application.Common.Services;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CivicHub.Application.Behaviours;

public class ValidationPipelineBehaviour<TRequest, TResponse>(
    ILogger<ValidationPipelineBehaviour<TRequest, TResponse>> logger,
    IValidationLocalizer validationLocalizer,
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
            .GroupBy(GetPropertyName)
            .SelectMany(group => group.Select(CreateError))
            .ToList();

        LogValidationFailures(errors);

        return (dynamic)errors;
    }

    private Error CreateError(ValidationFailure validationFailure)
    {
        var propertyName = GetPropertyName(validationFailure);
        var localizedPropertyValue = validationLocalizer.Translate(propertyName);
        var localizedErrorMessage = validationLocalizer.Translate(
            FormatErrorMessage(validationFailure),
            propertyName);
        var errorMessage = localizedErrorMessage.Replace(propertyName, localizedPropertyValue);
        return new Error(errorMessage, ErrorType.Validation);
    }

    private static string FormatErrorMessage(ValidationFailure validationFailure)
    {
        const string separator = "'";
        const string emptySpace = "";

        return validationFailure.ErrorMessage.Replace(separator, emptySpace);
    }

    private string GetLocalizedPropertyName(ValidationFailure validationFailure)
        => validationLocalizer.Translate(GetPropertyName(validationFailure));

    private static string GetPropertyName(ValidationFailure validationFailure)
    {
        const string key = "PropertyName";
        var formattedName = validationFailure.FormattedMessagePlaceholderValues[key];
        if (formattedName is not null)
        {
            return (string)formattedName;
        }

        return validationFailure.PropertyName;
    }

    private void LogValidationFailures(IEnumerable<Error> errors)
    {
        const string separator = " | ";
        var errorMessages = string.Join(separator, errors.Select(x => x.Message));

        logger.LogInformation("Validation failed for request: {RequestName}, Reason(s): {Reasons}",
            typeof(TRequest).Name, errorMessages);
    }
}