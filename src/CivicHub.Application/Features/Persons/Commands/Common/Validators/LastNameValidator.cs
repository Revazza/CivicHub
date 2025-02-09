using CivicHub.Application.Features.Persons.Commands.Common.Extensions;
using CivicHub.Domain.Persons;
using FluentValidation;

namespace CivicHub.Application.Features.Persons.Commands.Common.Validators;

public class LastNameValidator : AbstractValidator<string>
{
    private const string PropertyName = nameof(Person.LastName);

    public LastNameValidator()
    {
        RuleFor(firstName => firstName)
            .NotEmpty()
            .MinimumLength(PersonConstraints.LastNameMinLength)
            .MustBeOneWord(PropertyName)
            .MaximumLength(PersonConstraints.LastNameMaxLength)
            .MustBeEnglishOrGeorgian(PropertyName);
    }
}