using CivicHub.Api.Resources;
using CivicHub.Application.Common.Services;
using Microsoft.Extensions.Localization;
using Microsoft.IdentityModel.Tokens;

namespace CivicHub.Api.Services;

public class ValidationLocalizer(IStringLocalizer<ValidatorMessages> localizer) : IValidationLocalizer
{
    public string Translate(string key) => key.IsNullOrEmpty() ? key : localizer[key];
}