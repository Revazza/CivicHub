using CivicHub.Api.Contracts;
using Microsoft.Extensions.Localization;
using Microsoft.IdentityModel.Tokens;

namespace CivicHub.Api.Services;

public class Localizer<T>(IStringLocalizer<T> localizer) : ILocalizer<T>
{
    public string Translate(string key, params object[] arguments)
    {
        var localized = key.IsNullOrEmpty() ? key : localizer[key];

        if (arguments == null || arguments.Length == 0)
        {
            return localized;
        }
        
        return string.Format(localized, arguments);
    }
}