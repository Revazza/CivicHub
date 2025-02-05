using CivicHub.Application.Features.Persons.Commands.Common.Extensions;
using CivicHub.Domain.Cities;
using FluentValidation;

namespace CivicHub.Application.Features.Persons.Commands.Common.Validators;

public class CityCodeValidator : AbstractValidator<string>
{
    public CityCodeValidator()
    {
        RuleFor(cityCode => cityCode)
            .NotEmpty()
            .MinimumLength(CityConstraints.MinCityCodeLength)
            .MaximumLength(CityConstraints.MaxCityCodeLength)
            .MustBeEnglish();
    }
}