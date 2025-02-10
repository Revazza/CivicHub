using CivicHub.Application.Common.Services;
using CivicHub.Domain.Persons.Entities.PersonConnections;
using FluentValidation;

namespace CivicHub.Application.Features.PersonConnections.Commands.AddConnection;

public class AddConnectionCommandValidator : AbstractValidator<AddConnectionCommand>
{
    public AddConnectionCommandValidator(IValidationLocalizer localizer)
    {
        RuleFor(request => request.PersonId)
            .GreaterThan(0);

        RuleFor(request => request.OtherPersonId)
            .GreaterThan(0);

        RuleFor(request => request)
            .Must(request => request.PersonId != request.OtherPersonId)
            .WithMessage("PersonIdCannotMatchAnother");

        RuleFor(request => request.ConnectionType)
            .NotEmpty()
            .MaximumLength(PersonConnectionConstraints.MaxConnectionTypeLength);
    }
}