using System.Text.RegularExpressions;
using CivicHub.Domain.Persons;
using FluentValidation;

namespace CivicHub.Application.Features.Persons.Commands.Common.Validators;

public class PersonalNumberValidator : AbstractValidator<string>
{
    public PersonalNumberValidator()
    {
        RuleFor(personalNumber => personalNumber)
            .NotEmpty()
            .WithName(nameof(Person.PersonalNumber))
            .Length(PersonConstraints.PersonalNumberLength)
            .Must(HasOnlyDigits)
            .WithMessage($"{nameof(Person.PersonalNumber)} is invalid");
    }

    private static bool HasOnlyDigits(string personalNumber) => Regex.IsMatch(personalNumber, @"^\d{11}$");
}