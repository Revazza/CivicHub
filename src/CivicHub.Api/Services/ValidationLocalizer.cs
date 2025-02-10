using CivicHub.Api.Resources;
using CivicHub.Application.Common.Services;
using Microsoft.Extensions.Localization;
using Microsoft.IdentityModel.Tokens;

namespace CivicHub.Api.Services;

public class ValidationLocalizer(IStringLocalizer<ValidatorMessages> localizer) : IValidationLocalizer
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