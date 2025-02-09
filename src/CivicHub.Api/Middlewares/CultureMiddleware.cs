using System.Globalization;

namespace CivicHub.Api.Middlewares;

public class CultureMiddleware(ILogger<CultureMiddleware> logger) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var cultureName = context.Request.Headers.AcceptLanguage.FirstOrDefault()
                          ?? "en";

        var culture = new CultureInfo(cultureName);

        CultureInfo.CurrentCulture = culture;
        CultureInfo.CurrentUICulture = culture;
        
        logger.LogInformation("Culture has been set to {CultureName}", culture.Name);
        
        await next(context);
    }
}