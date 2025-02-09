using CivicHub.Application.Features.Persons.Common.Validators;
using CivicHub.Application.Features.Persons.Queries.Common.Validators;
using FluentValidation;

namespace CivicHub.Application.Features.Persons.Queries.DetailedSearchPerson;

public class DetailedSearchPersonQueryValidator : AbstractValidator<DetailedSearchPersonQuery>
{
    public DetailedSearchPersonQueryValidator()
    {
        RuleFor(x => x)
            .SetValidator(new SimpleSearchPersonQueryValidator());

        RuleFor(x => x.CityCode)
            .SetValidator(new CityCodeValidator());
    }
}