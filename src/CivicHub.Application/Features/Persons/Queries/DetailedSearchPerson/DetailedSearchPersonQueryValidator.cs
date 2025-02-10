using CivicHub.Application.Common.Extensions;
using CivicHub.Application.Features.Persons.Queries.Common.Validators;
using CivicHub.Domain.Cities;
using FluentValidation;

namespace CivicHub.Application.Features.Persons.Queries.DetailedSearchPerson;

public class DetailedSearchPersonQueryValidator : AbstractValidator<DetailedSearchPersonQuery>
{
    public DetailedSearchPersonQueryValidator()
    {
        RuleFor(x => x)
            .SetValidator(new SimpleSearchPersonQueryValidator());
        
        RuleFor(x => x.CityName)
            .MaximumLength(CityConstraints.MaxCityNameLength)
            .MustBeEnglishOrGeorgian()
            .When(x => x.CityName.IsNotNullOrEmpty());

        RuleFor(x => x.CityCode)
            .MaximumLength(CityConstraints.MaxCityCodeLength)
            .MustBeEnglish()
            .When(x => x.CityCode.IsNotNullOrEmpty());
    }
}