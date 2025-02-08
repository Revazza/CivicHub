using Serilog.Context;

namespace CivicHub.Api.Middlewares;

public class CorrelationMiddleware
    : IMiddleware
{
    private const string CorrelationId = "CorrelationId";
    
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        LogContext.PushProperty(CorrelationId, Guid.NewGuid());
        await next(context);
    }
}