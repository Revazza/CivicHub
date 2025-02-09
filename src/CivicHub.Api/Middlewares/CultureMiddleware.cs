using System.Globalization;

namespace CivicHub.Api.Middlewares;

public class CultureMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var cultureName = context.Request.Headers.AcceptLanguage.FirstOrDefault() 
                          ?? "en";
        
        var culture = new CultureInfo(cultureName);
        
        CultureInfo.CurrentCulture = culture;
        CultureInfo.CurrentUICulture = culture;

        await next(context);
    }
}