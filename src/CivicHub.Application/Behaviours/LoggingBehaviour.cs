using MediatR;
using Microsoft.Extensions.Logging;

namespace CivicHub.Application.Behaviours;

public class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Process started to handle {Reqest}", request);
        
        var response = await next();
        
        logger.LogInformation("Process completed to handle {RequestType}", typeof(TRequest).Name);

        return response;
    }
}