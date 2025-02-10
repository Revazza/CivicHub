using FluentValidation;

namespace CivicHub.Application.Features.Persons.Commands.DeletePerson;

public class DeletePersonCommandValidator: AbstractValidator<DeletePersonCommand>
{
    public DeletePersonCommandValidator()
    {
        RuleFor(person => person.PersonId)
            .GreaterThan(0);
    }
}