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
        
        RuleFor(person => person.PersonalNumber)
            .NotNull()
            .SetValidator(new PersonalNumberValidator());

        RuleFor(person => person.BirthDate)
            .SetValidator(new AgeValidator());
        
        RuleFor(person => person.CityCode)
            .NotNull()
            .SetValidator(new CityCodeValidator());
        
        RuleForEach(person => person.PhoneNumbers)
            .SetValidator(new PhoneNumberDtoValidator());
    }
}