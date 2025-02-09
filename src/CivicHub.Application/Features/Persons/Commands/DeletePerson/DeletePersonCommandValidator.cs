using CivicHub.Application.Features.Persons.Common.Validators;
using FluentValidation;

namespace CivicHub.Application.Features.Persons.Commands.DeletePerson;

public class DeletePersonCommandValidator: AbstractValidator<DeletePersonCommand>
{
    public DeletePersonCommandValidator()
    {
        RuleFor(person => person.PersonalNumber)
            .SetValidator(new PersonalNumberValidator());
    }
}