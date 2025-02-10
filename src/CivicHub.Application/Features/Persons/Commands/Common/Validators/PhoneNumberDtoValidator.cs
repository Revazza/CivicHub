using CivicHub.Application.Common.Extensions;
using CivicHub.Application.Common.Localization;
using CivicHub.Application.Features.Persons.Commands.Common.Dtos;
using CivicHub.Domain.Persons.ValueObjects.PhoneNumbers;
using FluentValidation;

namespace CivicHub.Application.Features.Persons.Commands.Common.Validators;

public class PhoneNumberDtoValidator : AbstractValidator<PhoneNumberDto>
{
    public PhoneNumberDtoValidator()
    {
        RuleFor(phone => phone.Number)
            .NotEmpty()
            .WithName(ValidatorMessagesKeys.PhoneNumber)
            .MinimumLength(PhoneNumberConstraints.MinNumberLength)
            .MaximumLength(PhoneNumberConstraints.MaxNumberLength)
            .MustContainOnlyDigits();

        RuleFor(phone => phone.Type)
            .IsInEnum()
            .WithName(ValidatorMessagesKeys.PhoneType);
    }
}