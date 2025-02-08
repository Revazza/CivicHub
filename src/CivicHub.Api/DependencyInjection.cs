using CivicHub.Api.ActionFilters;
using CivicHub.Api.Middlewares;
using Microsoft.AspNetCore.Mvc;

namespace CivicHub.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddApi(this IServiceCollection services)
    {
        services.AddScoped<FieldValidationFilter>();
        services.AddScoped<ValidationResultFilter>();
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });
        services.AddScoped<GlobalExceptionLoggingMiddleware>();
        return services;
    }
}