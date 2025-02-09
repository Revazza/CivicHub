using CivicHub.Application.Features.Persons.Commands.Common.Extensions;
using CivicHub.Domain.Cities;
using CivicHub.Domain.Persons;
using FluentValidation;

namespace CivicHub.Application.Features.Persons.Common.Validators;

public class CityCodeValidator : AbstractValidator<string>
{
    private const string PropertyName = nameof(Person.CityCode);
    
    public CityCodeValidator()
    {
        RuleFor(cityCode => cityCode)
            .NotEmpty()
            .MinimumLength(CityConstraints.MinCityCodeLength)
            .MaximumLength(CityConstraints.MaxCityCodeLength)
            .MustBeEnglish(PropertyName);
    }
}