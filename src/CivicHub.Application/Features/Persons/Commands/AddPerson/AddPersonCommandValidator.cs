using CivicHub.Application.Features.Persons.Commands.Common.Validators;
using FluentValidation;

namespace CivicHub.Application.Features.Persons.Commands.AddPerson;

public class AddPersonCommandValidator : AbstractValidator<AddPersonCommand>
{
    public AddPersonCommandValidator()
    {
        RuleFor(person => person.FirstName)
            .NotEmpty()
            .SetValidator(new FirstNameValidator());

        RuleFor(person => person.LastName)
            .NotEmpty()
            .SetValidator(new LastNameValidator());
        
        RuleFor(person => person.PersonalNumber)
            .NotEmpty()
            .SetValidator(new PersonalNumberValidator());

        RuleFor(person => person.BirthDate)
            .NotEmpty()
            .SetValidator(new AgeValidator());
        
        RuleFor(person => person.CityCode)
            .NotEmpty()
            .SetValidator(new CityCodeValidator());

        RuleFor(person => person.PhoneNumbers)
            .NotEmpty();
        
        RuleForEach(person => person.PhoneNumbers)
            .NotEmpty()
            .SetValidator(new PhoneNumberDtoValidator());
    }
}