using CivicHub.Application.Common.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CivicHub.Api.ActionFilters;

/// <summary>
/// An action filter which is typically used to handle cases
/// where input data does not conform to the expected type, format, or value.
/// For example
/// A field is expected to have a specific Enum value, but the client sends an invalid type or null value.
/// Reason for adding:
/// Most importantly such failures has their own response
/// {
///     "type":...,
///     "title":...,
///     "status":...,
///     "errors":...,
///     "traceId":...,
/// }
/// With this we avoid having two kind of response and stick with the <see cref="Result"/> response
/// </summary>
public class FieldValidationAttribute : IActionFilter
{
    private const string Separator = "; ";

    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.ModelState.IsValid)
        {
            return;
        }

        var allErrors = GetAllErrors(context);
        var errorMessage = string.Join(Separator, allErrors);
        var result = Result.Failure(new Error(errorMessage, ErrorType.Failure));
        context.Result = new UnprocessableEntityObjectResult(result);
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        // Left empty by intention
    }

    private static IEnumerable<string> GetAllErrors(ActionExecutingContext context)
        => context.ModelState
            .Where(ms => ms.Value.Errors.Count > 0)
            .SelectMany(ms => ms.Value.Errors.Select(e => $"{ms.Key}: {e.ErrorMessage}"));
}