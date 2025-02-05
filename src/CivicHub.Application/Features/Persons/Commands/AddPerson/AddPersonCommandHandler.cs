using CivicHub.Application.Common.Results;
using CivicHub.Application.Repositories;
using CivicHub.Domain.Locations.Exceptions;
using CivicHub.Domain.Persons;
using CivicHub.Domain.Persons.Exceptions;
using CivicHub.Domain.Persons.ValueObjects.PhoneNumbers;
using MediatR;

namespace CivicHub.Application.Features.Persons.Commands.AddPerson;

public class AddPersonCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<AddPersonCommand, Result<AddPersonResponse>>
{
    public async Task<Result<AddPersonResponse>> Handle(AddPersonCommand request, CancellationToken cancellationToken)
    {
        await ValidateAsync(request, cancellationToken);
        var person = ConvertToPerson(request);
        await unitOfWork.PersonRepository.InsertAsync(person, cancellationToken);
        return new AddPersonResponse();
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

    private static Person ConvertToPerson(AddPersonCommand request)
        => new()
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            PersonalNumber = request.PersonalNumber,
            Gender = request.Gender,
            BirthDate = request.BirthDate,
            LocationId = request.LocationId,
            PhoneNumbers = ConvertToPhoneNumbers(request.PhoneNumbers),
            ConnectedTo = [],
            Connections = [],
        };

    private static List<PhoneNumber> ConvertToPhoneNumbers(List<PhoneNumberDto> phoneNumbers)
        => phoneNumbers
            .Select(phoneNumber => new PhoneNumber
            {
                CountryCode = phoneNumber.CountryCode,
                AreaCode = phoneNumber.AreaCode,
                Number = phoneNumber.Number,
                Type = phoneNumber.Type,
            }).ToList();
}