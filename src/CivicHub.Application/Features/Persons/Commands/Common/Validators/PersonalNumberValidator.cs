using CivicHub.Application.Features.Persons.Commands.Common.Extensions;
using CivicHub.Domain.Persons;
using FluentValidation;

namespace CivicHub.Application.Features.Persons.Commands.Common.Validators;

public class PersonalNumberValidator : AbstractValidator<string>
{
    private const string PropertyName = nameof(Person.PersonalNumber);
    
    public PersonalNumberValidator()
    {
        RuleFor(personalNumber => personalNumber)
            .NotEmpty()
            .Length(PersonConstraints.PersonalNumberLength)
            .MustContainOnlyDigits(PropertyName);
    }
}