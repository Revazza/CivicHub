using CivicHub.Application.Common.Extensions;
using CivicHub.Application.Common.Localization;
using CivicHub.Application.Features.Persons.Commands.Common.Validators;
using FluentValidation;

namespace CivicHub.Application.Features.Persons.Commands.UpdatePerson;

public class UpdatePersonCommandValidator : AbstractValidator<UpdatePersonCommand>
{
    public UpdatePersonCommandValidator()
    {
        RuleFor(person => person.FirstName)
            .SetValidator(new FirstNameValidator())
            .When(person => person.FirstName.IsNotNullOrEmpty());

        RuleFor(person => person.LastName)
            .SetValidator(new LastNameValidator())
            .When(person => person.LastName.IsNotNullOrEmpty());

        RuleFor(person => person)
            .Must(person => person.FirstName.HasMatchingLanguageWith(person.LastName))
            .WithMessage(ValidatorMessagesKeys.FirstAndLastNameDontHaveMatchingLanguages)
            .When(person => person.FirstName.IsNotNullOrEmpty() && person.LastName.IsNotNullOrEmpty());


        RuleFor(person => person.PersonalNumber)
            .SetValidator(new PersonalNumberValidator())
            .When(person => person.PersonalNumber.IsNotNullOrEmpty());

        RuleFor(person => person.BirthDate)
            .SetValidator(new AgeValidator())
            .When(person => person.BirthDate.IsNotDefault());

        RuleFor(person => person.CityCode)
            .SetValidator(new CityCodeValidator())
            .When(person => person.CityCode.IsNotNullOrEmpty());

        RuleForEach(person => person.PhoneNumbers)
            .SetValidator(new PhoneNumberDtoValidator())
            .When(person => person.PhoneNumbers.IsNotNullOrEmpty());
    }
}