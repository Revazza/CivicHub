using CivicHub.Api.ActionFilters;
using CivicHub.Api.Contracts;
using CivicHub.Api.Middlewares;
using CivicHub.Api.Options;
using CivicHub.Api.Resources;
using CivicHub.Api.Services;
using CivicHub.Application.Common.Localization;
using Microsoft.AspNetCore.Mvc;

namespace CivicHub.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddApi(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<FieldValidationFilter>();
        services.AddScoped<ValidationResultFilter>();
        services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });
        services.AddScoped<CorrelationMiddleware>();
        services.AddScoped<GlobalExceptionLoggingMiddleware>();
        services.AddLocalizationServices(configuration);

        return services;
    }

    public static void SetupAndUseRequestLocalization(this WebApplication app)
    {
        var options = app.Configuration.GetSection(LocalizationOptions.SectionName).Get<LocalizationOptions>();

        var localizationOptions = new RequestLocalizationOptions()
            .SetDefaultCulture(options.DefaultCulture)
            .AddSupportedCultures(options.SupportedCultures.ToArray())
            .AddSupportedUICultures(options.SupportedCultures.ToArray());

        app.UseRequestLocalization(localizationOptions);
    }

    private static void AddLocalizationServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddLocalization();
        services.AddScoped<LanguageMiddleware>();
        services.AddTransient<IValidationLocalizer, ValidationLocalizer>();
        services.AddTransient<ILocalizer<DomainExceptionMessages>, Localizer<DomainExceptionMessages>>();

        var localizationOptions = configuration.GetSection(LocalizationOptions.SectionName).Get<LocalizationOptions>();

        services.AddRequestLocalization(options =>
        {
            options.SetDefaultCulture(localizationOptions.DefaultCulture)
                .AddSupportedCultures(localizationOptions.SupportedCultures.ToArray())
                .AddSupportedUICultures(localizationOptions.SupportedCultures.ToArray());
        });
    }
}