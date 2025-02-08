using CivicHub.Application.Features.Persons.Commands.Common.Dtos;
using CivicHub.Application.Features.Persons.Commands.Common.Extensions;
using CivicHub.Domain.Persons.ValueObjects.PhoneNumbers;
using FluentValidation;

namespace CivicHub.Application.Features.Persons.Commands.Common.Validators;

public class PhoneNumberDtoValidator : AbstractValidator<PhoneNumberDto>
{
    private const string CountryCode = nameof(PhoneNumberDto.CountryCode);
    private const string AreaCode = nameof(PhoneNumberDto.AreaCode);
    private const string Number = nameof(PhoneNumberDto.Number);
    
    public PhoneNumberDtoValidator()
    {
        RuleFor(phone => phone.CountryCode)
            .NotEmpty()
            .MinimumLength(PhoneNumberConstraints.MinCountryCodeLength)
            .MaximumLength(PhoneNumberConstraints.MaxCountryCodeLength)
            .MustContainOnlyDigits(CountryCode);

        RuleFor(phone => phone.AreaCode)
            .NotEmpty()
            .MinimumLength(PhoneNumberConstraints.MinAreaCodeLength)
            .MaximumLength(PhoneNumberConstraints.MaxAreaCodeLength)
            .MustContainOnlyDigits(AreaCode);

        RuleFor(phone => phone.Number)
            .NotEmpty()
            .MinimumLength(PhoneNumberConstraints.MinNumberLength)
            .MaximumLength(PhoneNumberConstraints.MaxNumberLength)
            .MustContainOnlyDigits(Number);
    }
}