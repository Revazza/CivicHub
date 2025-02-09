using CivicHub.Application.Common.Extensions;
using CivicHub.Application.Features.Persons.Queries.SimpleSearchPerson;
using CivicHub.Domain.Persons;
using FluentValidation;

namespace CivicHub.Application.Features.Persons.Queries.Common.Validators;

public class SimpleSearchPersonQueryValidator : AbstractValidator<SimpleSearchPersonQuery>
{
    private const int MinPageNumber = 1;
    private const int MinPageSize = 1;
    private const int MaxPageSize = 100;
    
    public SimpleSearchPersonQueryValidator()
    {
        RuleFor(person => person.FirstName)
            .MaximumLength(PersonConstraints.FirstNameMaxLength)
            .When(person => person.FirstName.IsNotNullOrEmpty());

        RuleFor(person => person.LastName)
            .MaximumLength(PersonConstraints.LastNameMaxLength)
            .When(person => person.LastName.IsNotNullOrEmpty());

        RuleFor(person => person.PersonalNumber)
            .MaximumLength(PersonConstraints.PersonalNumberLength)
            .When(person => person.PersonalNumber.IsNotNullOrEmpty());

        RuleFor(person => person.PageSize)
            .GreaterThanOrEqualTo(MinPageSize)
            .LessThanOrEqualTo(MaxPageSize);
        
        RuleFor(person => person.PageNumber)
            .GreaterThanOrEqualTo(MinPageNumber);
    }
}