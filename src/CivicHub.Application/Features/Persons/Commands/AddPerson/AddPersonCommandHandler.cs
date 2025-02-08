using CivicHub.Application.Common.Results;
using CivicHub.Application.Repositories;
using CivicHub.Domain.Cities.Exceptions;
using CivicHub.Domain.Persons;
using CivicHub.Domain.Persons.Exceptions;
using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CivicHub.Application.Features.Persons.Commands.AddPerson;

public class AddPersonCommandHandler(IUnitOfWork unitOfWork, ILogger<AddPersonCommandHandler> logger)
    : IRequestHandler<AddPersonCommand, Result>
{
    public async Task<Result> Handle(AddPersonCommand request, CancellationToken cancellationToken)
    {
        await ValidateAsync(request, cancellationToken);

        var person = request.Adapt<Person>();

        logger.LogInformation("Person with PersonalNumber: {PersonalNumber} is being inserted", request.PersonalNumber);

        await unitOfWork.PersonRepository.InsertAsync(person, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Person with PersonalNumber: {PersonalNumber} successfully added",
            request.PersonalNumber);

        return Result.Success();
    }

    private async Task ValidateAsync(AddPersonCommand request, CancellationToken cancellationToken)
    {
        await EnsurePersonDoesntExistAsync(request.PersonalNumber, cancellationToken);
        await EnsureCityExistsAsync(request.CityCode, cancellationToken);
    }

    private async Task EnsurePersonDoesntExistAsync(string personalNumber, CancellationToken cancellationToken)
    {
        var exists = await unitOfWork.PersonRepository.DoesExistAsync(personalNumber, cancellationToken);
        if (exists)
        {
            throw new PersonAlreadyExistsException(personalNumber);
        }
    }

    private async Task EnsureCityExistsAsync(string cityCode, CancellationToken cancellationToken)
    {
        var exists = await unitOfWork.CityRepository.DoesExistAsync(cityCode, cancellationToken);
        if (!exists)
        {
            throw new CityDoesntExistException(cityCode);
        }
    }
}