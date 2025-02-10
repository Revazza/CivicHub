using CivicHub.Application.Common.Extensions;
using CivicHub.Application.Common.Localization;
using CivicHub.Domain.Cities;
using FluentValidation;

namespace CivicHub.Application.Features.Persons.Commands.Common.Validators;

public class CityCodeValidator : AbstractValidator<string>
{
    public CityCodeValidator()
    {
        RuleFor(cityCode => cityCode)
            .NotEmpty()
            .WithName(ValidatorMessagesKeys.CityCode)
            .MinimumLength(CityConstraints.MinCityCodeLength) 
            .MaximumLength(CityConstraints.MaxCityCodeLength)
            .MustBeEnglish();
    }
}