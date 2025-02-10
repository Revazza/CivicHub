using CivicHub.Application.Common.Results;
using CivicHub.Application.Repositories;
using CivicHub.Domain.Persons.Entities.PersonConnections;
using CivicHub.Domain.Persons.Entities.PersonConnections.Exceptions;
using CivicHub.Domain.Persons.Exceptions;
using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CivicHub.Application.Features.PersonConnections.Commands.AddConnection;

public class AddConnectionCommandHandler(IUnitOfWork unitOfWork, ILogger<AddConnectionCommandHandler> logger)
    : IRequestHandler<AddConnectionCommand, Result>
{
    public async Task<Result> Handle(AddConnectionCommand request, CancellationToken cancellationToken)
    {
        await ValidateAsync(request, cancellationToken);

        var connection = request.Adapt<PersonConnection>();

        await unitOfWork.PersonConnectionRepository.InsertAsync(connection, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Connection {ConnectionId} has been created", connection.Id);
        return Result.Success();
    }

    private async Task ValidateAsync(AddConnectionCommand request, CancellationToken cancellationToken)
    {
        await EnsurePersonsExistsAsync(request, cancellationToken);
        await EnsureConnectionDoesntExistsAsync(request, cancellationToken);
    }

    private async Task EnsurePersonsExistsAsync(AddConnectionCommand request, CancellationToken cancellationToken)
    {
        var bothExists = await unitOfWork
            .PersonRepository
            .DoBothPersonsExistAsync(request.PersonId, request.OtherPersonId, cancellationToken);

        if (!bothExists)
        {
            throw new PersonDoesntExistException(request.PersonId, request.OtherPersonId);
        }
    }

    private async Task EnsureConnectionDoesntExistsAsync(
        AddConnectionCommand request,
        CancellationToken cancellationToken)
    {
        var exists = await unitOfWork.PersonConnectionRepository.DoesConnectionExistsAsync(
            request.PersonId,
            request.OtherPersonId,
            request.ConnectionType,
            cancellationToken);

        if (exists)
        {
            throw new PersonConnectionExistsException(request.ConnectionType);
        }
    }
}