using CivicHub.Application.Common.Results;
using CivicHub.Application.Repositories;
using CivicHub.Domain.Locations.Exceptions;
using CivicHub.Domain.Persons;
using CivicHub.Domain.Persons.Exceptions;
using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CivicHub.Application.Features.Persons.Commands.AddPerson;

public class AddPersonCommandHandler(IUnitOfWork unitOfWork, ILogger<AddPersonCommandHandler> logger)
    : IRequestHandler<AddPersonCommand, Result<AddPersonResponse>>
{
    public async Task<Result<AddPersonResponse>> Handle(AddPersonCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Process started to handle {RequestType} for PersonalNumber: {PersonalNumber}",
            nameof(AddPersonCommand), request.PersonalNumber);

        await ValidateAsync(request, cancellationToken);

        var person = request.Adapt<Person>();

        logger.LogInformation("Person with PersonalNumber: {PersonalNumber} is being inserted", request.PersonalNumber);

        await unitOfWork.PersonRepository.InsertAsync(person, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Person with PersonalNumber: {PersonalNumber} successfully added",
            request.PersonalNumber);

        logger.LogInformation("Process ended to handle {RequestType} for PersonalNumber: {PersonalNumber}",
            nameof(AddPersonCommand), request.PersonalNumber);

        return person.Adapt<AddPersonResponse>();
    }

    private async Task ValidateAsync(AddPersonCommand request, CancellationToken cancellationToken)
    {
        await EnsurePersonDoesntExistAsync(request.PersonalNumber, cancellationToken);
        await EnsureLocationExistsAsync(request.LocationId, cancellationToken);
    }

    private async Task EnsurePersonDoesntExistAsync(string personalNumber, CancellationToken cancellationToken)
    {
        var exists = await unitOfWork.PersonRepository.DoesExistAsync(personalNumber, cancellationToken);
        if (exists)
        {
            throw new PersonAlreadyExistsException(personalNumber);
        }
    }

    private async Task EnsureLocationExistsAsync(Guid locationId, CancellationToken cancellationToken)
    {
        var exists = await unitOfWork.LocationRepository.DoesExistAsync(locationId, cancellationToken);
        if (!exists)
        {
            throw new LocationDoesntExistException(locationId);
        }
    }
}