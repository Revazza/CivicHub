using CivicHub.Api.Middlewares;

namespace CivicHub.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddApi(this IServiceCollection services)
    {
        services.AddScoped<GlobalExceptionLoggingMiddleware>();
        return services;
    }
}