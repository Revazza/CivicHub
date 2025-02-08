using CivicHub.Application.Common.Results;
using CivicHub.Application.Repositories;
using CivicHub.Domain.Persons.Entities.PersonConnections;
using CivicHub.Domain.Persons.Entities.PersonConnections.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CivicHub.Application.Features.PersonConnections.Commands.RemoveConnection;

public class RemoveConnectionCommandHandler(IUnitOfWork unitOfWork, ILogger<RemoveConnectionCommandHandler> logger)
    : IRequestHandler<RemoveConnectionCommand, Result>
{
    public async Task<Result> Handle(RemoveConnectionCommand request, CancellationToken cancellationToken)
    {
        var connection = await GetConnectionAsync(request.ConnectionId);

        unitOfWork.PersonConnectionRepository.Delete(connection);

        await unitOfWork.SaveChangesAsync(cancellationToken);
        logger.LogInformation("Connection by {ConnectionId} was successfully removed", request.ConnectionId);
        return Result.Success();
    }

    private async Task<PersonConnection> GetConnectionAsync(Guid connectionId)
    {
        var connection = await unitOfWork
                             .PersonConnectionRepository
                             .GetByIdAsync(connectionId)
                         ?? throw new PersonConnectionDoesntExistException(connectionId);

        return connection;
    }
}