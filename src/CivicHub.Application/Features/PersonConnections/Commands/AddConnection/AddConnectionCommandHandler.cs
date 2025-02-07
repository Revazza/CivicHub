using CivicHub.Application.Common.Results;
using CivicHub.Application.Repositories;
using CivicHub.Domain.Persons.Entities.PersonConnections;
using CivicHub.Domain.Persons.Entities.PersonConnections.Exceptions;
using MediatR;

namespace CivicHub.Application.Features.PersonConnections.Commands.AddConnection;

public class AddConnectionCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<AddConnectionCommand, Result>
{
    public async Task<Result> Handle(AddConnectionCommand request, CancellationToken cancellationToken)
    {
        await EnsureConnectionDoesntExistsAsync(request, cancellationToken);

        var connection = new PersonConnection
        {
            PersonId = request.PersonId,
            ConnectedPersonId = request.TargetPersonId
        };
        
        await unitOfWork.PersonConnectionRepository.InsertAsync(connection, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }

    private async Task EnsureConnectionDoesntExistsAsync(
        AddConnectionCommand request,
        CancellationToken cancellationToken)
    {
        var exists = await unitOfWork.PersonConnectionRepository.DoesConnectionExistsAsync(
            request.PersonId,
            request.TargetPersonId,
            cancellationToken);

        if (exists)
        {
            throw new PersonConnectionExistsException(request.PersonId, request.TargetPersonId);
        }
    }
}