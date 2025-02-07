using CivicHub.Application.Features.Persons.Commands.Common.Dtos;
using CivicHub.Application.Features.Persons.Commands.Common.Extensions;
using CivicHub.Domain.Persons.ValueObjects.PhoneNumbers;
using FluentValidation;

namespace CivicHub.Application.Features.Persons.Commands.Common.Validators;

public class PhoneNumberDtoValidator : AbstractValidator<PhoneNumberDto>
{
    public PhoneNumberDtoValidator()
    {
        RuleFor(phone => phone.CountryCode)
            .NotEmpty()
            .MinimumLength(PhoneNumberConstraints.MinCountryCodeLength)
            .MaximumLength(PhoneNumberConstraints.MaxCountryCodeLength)
            .MustContainOnlyDigits();

        RuleFor(phone => phone.AreaCode)
            .NotEmpty()
            .MinimumLength(PhoneNumberConstraints.MinAreaCodeLength)
            .MaximumLength(PhoneNumberConstraints.MaxAreaCodeLength)
            .MustContainOnlyDigits();

        RuleFor(phone => phone.Number)
            .NotEmpty()
            .MinimumLength(PhoneNumberConstraints.MinNumberLength)
            .MaximumLength(PhoneNumberConstraints.MaxNumberLength)
            .MustContainOnlyDigits();
    }
}