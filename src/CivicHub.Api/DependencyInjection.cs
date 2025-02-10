using CivicHub.Api.ActionFilters;
using CivicHub.Api.Middlewares;
using CivicHub.Api.Services;
using CivicHub.Application.Common.Services;
using Microsoft.AspNetCore.Mvc;

namespace CivicHub.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddApi(this IServiceCollection services)
    {
        services.AddScoped<LanguageMiddleware>();
        services.AddScoped<FieldValidationFilter>();
        services.AddScoped<ValidationResultFilter>();
        services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });
        services.AddScoped<CorrelationMiddleware>();
        services.AddScoped<GlobalExceptionLoggingMiddleware>();
        services.AddTransient<IValidationLocalizer, ValidationLocalizer>();

        services.AddLocalization();
        services.AddRequestLocalization(options =>
        {
            var supportedCultures = new[] { "en", "ka" };
            options.SetDefaultCulture(supportedCultures[0])
                .AddSupportedCultures(supportedCultures)
                .AddSupportedUICultures(supportedCultures);
        });

        return services;
    }
}