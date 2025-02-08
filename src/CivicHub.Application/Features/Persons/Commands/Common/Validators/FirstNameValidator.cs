using CivicHub.Application.Features.Persons.Commands.Common.Extensions;
using CivicHub.Domain.Persons;
using FluentValidation;

namespace CivicHub.Application.Features.Persons.Commands.Common.Validators;

public class FirstNameValidator : AbstractValidator<string>
{
    private const string PropertyName = nameof(Person.FirstName);

    public FirstNameValidator()
    {
        RuleFor(firstName => firstName)
            .NotEmpty()
            .MinimumLength(PersonConstraints.FirstNameMinLength)
            .MaximumLength(PersonConstraints.FirstNameMaxLength)
            .MustBeOneWord(PropertyName)
            .MustBeEnglishOrGeorgian(PropertyName);
    }
}