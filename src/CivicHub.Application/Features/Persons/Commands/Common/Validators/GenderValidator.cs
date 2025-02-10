using CivicHub.Application.Common.Localization;
using CivicHub.Domain.Persons.Enums;
using FluentValidation;

namespace CivicHub.Application.Features.Persons.Commands.Common.Validators;

public class GenderValidator : AbstractValidator<Gender>
{
    public GenderValidator()
    {
        RuleFor(gender => gender)
            .IsInEnum()
            .Must(gender => gender != Gender.NotSpecified)
            .WithMessage(ValidatorMessagesKeys.GenderMustBeSpecified);
    }
}