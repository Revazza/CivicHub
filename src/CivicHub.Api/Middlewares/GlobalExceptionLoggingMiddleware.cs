using System.Net;
using CivicHub.Application.Common.Results;
using CivicHub.Domain.Common.Exceptions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace CivicHub.Api.Middlewares;

public class GlobalExceptionLoggingMiddleware(ILogger<GlobalExceptionLoggingMiddleware> logger) : IMiddleware
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

            await WriteDomainExceptionResponseAsync(ex, context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled error occurred while processing the request: {ErrorMessage}", ex.Message);

            await WriteUnhandledExceptionResponseAsync(ex, context);
        }
    }

    private static async Task WriteDomainExceptionResponseAsync(DomainException ex, HttpContext context)
    {
        SetResponse(HttpStatusCode.BadRequest, context);
        await WriteResponseAsync(ErrorType.Failure, ex, context);
    }

    private static async Task WriteUnhandledExceptionResponseAsync(Exception ex, HttpContext context)
    {
        SetResponse(HttpStatusCode.InternalServerError, context);
        await WriteResponseAsync(ErrorType.Problem, ex, context);
    }

    private static void SetResponse(HttpStatusCode statusCode, HttpContext context)
    {
        context.Response.StatusCode = (int)statusCode;
        context.Response.ContentType = "application/json";
    }

    private static async Task WriteResponseAsync(ErrorType errorType, Exception ex, HttpContext context)
    {
        var response = Result.Failure(new Error(ex.Message, errorType));
        var responseJson = JsonConvert.SerializeObject(response, JsonSettings);
        await context.Response.WriteAsync(responseJson);
    }
}