using System.Globalization;
using Microsoft.AspNetCore.Localization;

namespace CivicHub.Api.Middlewares;

/// <summary>
///                                         Important
/// The task required manually setting the current culture using middleware
/// However, this is automatically handled by <see cref="RequestLocalizationMiddleware" />
/// when configuring <see cref="Microsoft.Extensions.DependencyInjection.RequestLocalizationServiceCollectionExtensions.AddRequestLocalization(IServiceCollection)" />.
/// </summary>
/// <remarks>
/// The AddRequestLocalization method registers localization services,
/// which ensuring that the correct CultureInfo.CurrentCulture is set automatically
/// based on request parameters (headers, query or cookies)
/// </remarks>
public class LanguageMiddleware(ILogger<LanguageMiddleware> logger) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        logger.LogInformation("Current Culture: {Culture}", CultureInfo.CurrentCulture);

        await next(context);
    }
}