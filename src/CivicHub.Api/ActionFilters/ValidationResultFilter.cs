using CivicHub.Application.Common.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CivicHub.Api.ActionFilters;

public class ValidationResultFilter : IResultFilter
{
    private const string Separator = "; ";

    public void OnResultExecuting(ResultExecutingContext context)
    {
        if (context.ModelState.IsValid)
        {
            return;
        }

        var objectResult = context.Result as ObjectResult;
        if (IsResponseDefined(objectResult))
        {
            return;
        }

        var allErrors = GetAllErrors(context);
        var errorMessage = string.Join(Separator, allErrors);
        var result = Result.Failure(new Error(errorMessage, ErrorType.Failure));
        context.Result = new UnprocessableEntityObjectResult(result);
    }

    public void OnResultExecuted(ResultExecutedContext context)
    {
    }

    private static bool IsResponseDefined(ObjectResult objectResult)
        => objectResult?.Value is Result;

    private static IEnumerable<string> GetAllErrors(ResultExecutingContext context)
        => context.ModelState
            .Where(ms => ms.Value.Errors.Count > 0)
            .SelectMany(ms => ms.Value.Errors.Select(e => e.Exception?.Message ?? e.ErrorMessage));
}