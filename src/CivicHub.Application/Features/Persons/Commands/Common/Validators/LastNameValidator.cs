using CivicHub.Application.Features.Persons.Commands.Common.Extensions;
using CivicHub.Domain.Persons;
using FluentValidation;

namespace CivicHub.Application.Features.Persons.Commands.Common.Validators;

public class LastNameValidator : AbstractValidator<string>
{
    public LastNameValidator()
    {
        RuleFor(firstName => firstName)
            .NotEmpty()
            .MinimumLength(PersonConstraints.LastNameMinLength)
            .MustBeOneWord()
            .MaximumLength(PersonConstraints.LastNameMaxLength)
            .MustBeEnglishOrGeorgian();
    }
}