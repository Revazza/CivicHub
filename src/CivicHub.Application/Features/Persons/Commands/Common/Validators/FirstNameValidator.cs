using CivicHub.Application.Features.Persons.Commands.Common.Extensions;
using CivicHub.Domain.Persons;
using FluentValidation;

namespace CivicHub.Application.Features.Persons.Commands.Common.Validators;

public class FirstNameValidator : AbstractValidator<string>
{

    public FirstNameValidator()
    {
        RuleFor(firstName => firstName)
            .NotEmpty()
            .MinimumLength(PersonConstraints.FirstNameMinLength)
            .MaximumLength(PersonConstraints.FirstNameMaxLength)
            .MustBeOneWord()
            .MustBeEnglishOrGeorgian();
    }
}