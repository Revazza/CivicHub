using System.Net;
using CivicHub.Api.Contracts;
using CivicHub.Api.Resources;
using CivicHub.Application.Common.Results;
using CivicHub.Domain.Common.Exceptions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace CivicHub.Api.Middlewares;

public class GlobalExceptionLoggingMiddleware(
    ILogger<GlobalExceptionLoggingMiddleware> logger,
    ILocalizer<DomainExceptionMessages> localizer) : IMiddleware
{
    private static readonly JsonSerializerSettings JsonSettings = new()
    {
        ContractResolver = new CamelCasePropertyNamesContractResolver()
    };

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (DomainException ex)
        {
            logger.LogWarning(ex, "Domain validation error occured while processing the request: {ErrorMessage}"
                , ex.Message);

            await WriteDomainExceptionResponseAsync(ex, context, ex.LocalizationArguments);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled error occurred while processing the request: {ErrorMessage}", ex.Message);

            await WriteUnhandledExceptionResponseAsync(ex, context);
        }
    }

    private async Task WriteDomainExceptionResponseAsync(
        DomainException ex,
        HttpContext context,
        params object[] arguments)
    {
        SetResponse(HttpStatusCode.BadRequest, context);
        await WriteResponseAsync(ErrorType.Failure, ex, context, arguments);
    }

    private async Task WriteUnhandledExceptionResponseAsync(Exception ex, HttpContext context)
    {
        SetResponse(HttpStatusCode.InternalServerError, context);
        await WriteResponseAsync(ErrorType.Problem, ex, context);
    }

    private static void SetResponse(HttpStatusCode statusCode, HttpContext context)
    {
        context.Response.StatusCode = (int)statusCode;
        context.Response.ContentType = "application/json";
    }

    private async Task WriteResponseAsync(
        ErrorType errorType,
        Exception ex,
        HttpContext context,
        params object[] arguments)
    {
        var response = Result.Failure(new Error(localizer.Translate(ex.Message, arguments), errorType));
        var responseJson = JsonConvert.SerializeObject(response, JsonSettings);
        await context.Response.WriteAsync(responseJson);
    }
}