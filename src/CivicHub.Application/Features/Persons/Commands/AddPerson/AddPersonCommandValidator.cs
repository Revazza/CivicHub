using CivicHub.Application.Common.Extensions;
using CivicHub.Application.Common.Localization;
using CivicHub.Application.Features.Persons.Commands.Common.Validators;
using FluentValidation;

namespace CivicHub.Application.Features.Persons.Commands.AddPerson;

public class AddPersonCommandValidator : AbstractValidator<AddPersonCommand>
{
    public AddPersonCommandValidator()
    {
        RuleFor(person => person.FirstName)
            .NotNull()
            .SetValidator(new FirstNameValidator());

        RuleFor(person => person.LastName)
            .NotNull()
            .SetValidator(new LastNameValidator());

        RuleFor(person => person)
            .Must(person => person.FirstName.HasMatchingLanguageWith(person.LastName))
            .WithMessage(ValidatorMessagesKeys.FirstAndLastNameDontHaveMatchingLanguages);
        
        RuleFor(person => person.PersonalNumber)
            .NotNull()
            .SetValidator(new PersonalNumberValidator());

        RuleFor(person => person.Gender)
            .NotNull()
            .SetValidator(new GenderValidator());
        
        RuleFor(person => person.BirthDate)
            .NotNull()
            .SetValidator(new AgeValidator());
        
        RuleFor(person => person.CityCode)
            .NotNull()
            .SetValidator(new CityCodeValidator());

        RuleFor(person => person.PhoneNumbers)
            .NotEmpty();
        
        RuleForEach(person => person.PhoneNumbers)
            .NotEmpty()
            .SetValidator(new PhoneNumberDtoValidator());
    }
}