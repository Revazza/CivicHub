using CivicHub.Application.Features.Persons.Commands.Common.Validators;
using FluentValidation;

namespace CivicHub.Application.Features.Persons.Commands.AddPerson;

public class AddPersonCommandValidator : AbstractValidator<AddPersonCommand>
{
    public AddPersonCommandValidator()
    {
        RuleFor(person => person.FirstName)
            .SetValidator(new FirstNameValidator());

        RuleFor(person => person.LastName)
            .SetValidator(new LastNameValidator());
        
        RuleFor(person => person.PersonalNumber)
            .SetValidator(new PersonalNumberValidator());
    }
}