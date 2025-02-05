using CivicHub.Middlewares;

namespace CivicHub;

public static class DependencyInjection
{
    public static IServiceCollection AddApi(this IServiceCollection services)
    {
        services.AddScoped<GlobalExceptionLoggingMiddleware>();
        return services;
    }
}