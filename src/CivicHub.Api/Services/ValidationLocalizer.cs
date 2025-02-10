using CivicHub.Api.Resources;
using CivicHub.Application.Common.Localization;
using Microsoft.Extensions.Localization;

namespace CivicHub.Api.Services;

public class ValidationLocalizer(IStringLocalizer<ValidatorMessages> localizer)
    : Localizer<ValidatorMessages>(localizer), IValidationLocalizer
{
}