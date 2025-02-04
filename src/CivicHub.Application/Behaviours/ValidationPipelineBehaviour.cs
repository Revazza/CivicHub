using CivicHub.Application.Common.Results;
using FluentValidation;
using MediatR;

namespace CivicHub.Application.Behaviours;

public class ValidationPipelineBehaviour<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IValidator<TRequest>? _validator;

    public ValidationPipelineBehaviour(IValidator<TRequest>? validator = null)
    {
        _validator = validator;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (_validator is null)
        {
            return await next();
        }

        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (validationResult.IsValid)
        {
            return await next();
        }

        var errors = validationResult.Errors
            .GroupBy(validationFailure => validationFailure.PropertyName)
            .SelectMany(group =>
                group.Select(validationFailure => new Error(validationFailure.ErrorMessage, ErrorType.Validation)))
            .ToList();

        return (dynamic)errors;
    }
}