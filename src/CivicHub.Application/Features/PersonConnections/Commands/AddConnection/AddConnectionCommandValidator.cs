using FluentValidation;

namespace CivicHub.Application.Features.PersonConnections.Commands.AddConnection;

public class AddConnectionCommandValidator : AbstractValidator<AddConnectionCommand>
{
    public AddConnectionCommandValidator()
    {
        RuleFor(request => request.PersonId)
            .GreaterThan(0);
        
        RuleFor(request => request.TargetPersonId)
            .GreaterThan(0);
    }
}