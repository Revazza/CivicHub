using CivicHub.Application.Common.Extensions;
using CivicHub.Application.Common.Results;
using CivicHub.Application.Features.Persons.Commands.Common.Dtos;
using CivicHub.Application.Repositories;
using CivicHub.Domain.Cities.Exceptions;
using CivicHub.Domain.Persons;
using CivicHub.Domain.Persons.Enums;
using CivicHub.Domain.Persons.Exceptions;
using CivicHub.Domain.Persons.ValueObjects.PhoneNumbers.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CivicHub.Application.Features.Persons.Commands.UpdatePerson;

public class UpdatePersonCommandHandler(IUnitOfWork unitOfWork, ILogger<UpdatePersonCommandHandler> logger)
    : IRequestHandler<UpdatePersonCommand, Result>
{
    public async Task<Result> Handle(UpdatePersonCommand request,
        CancellationToken cancellationToken)
    {
        var person = await unitOfWork
                         .PersonRepository
                         .GetForUpdateAsync(request.PersonalNumber, cancellationToken)
                     ?? throw new PersonDoesntExistException(request.PersonalNumber);

        await UpdateAsync(person, request, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    private async Task UpdateAsync(Person person, UpdatePersonCommand request, CancellationToken cancellationToken)
    {
        UpdateFirstNameIfSpecified(person, request.FirstName);
        UpdateLastNameIfSpecified(person, request.LastName);
        UpdateBirthDateIfSpecified(person, request.BirthDate);
        UpdatePhoneNumbersIfSpecified(person, request.PhoneNumbers);
        UpdatePersonalNumberIfSpecified(person, request.PersonalNumber);
        UpdateGenderIfSpecified(person, request.Gender);
        await UpdateCityCodeIfSpecifiedAsync(person, request.CityCode, cancellationToken);
    }

    private void UpdateFirstNameIfSpecified(Person person, string firstName)
    {
        if (firstName.IsNullOrEmpty() || person.FirstName == firstName)
        {
            return;
        }

        person.FirstName = firstName.ToLower().Capitalize();
        logger.LogInformation("{PropertyName} is updated", nameof(Person.FirstName));
    }

    private void UpdateLastNameIfSpecified(Person person, string lastName)
    {
        if (lastName.IsNullOrEmpty() || person.LastName == lastName)
        {
            return;
        }

        person.LastName = lastName.ToLower().Capitalize();
        logger.LogInformation("{PropertyName} is updated", nameof(Person.LastName));
    }

    private void UpdateBirthDateIfSpecified(Person person, DateTime birthDate)
    {
        if (birthDate.IsDefault() || person.BirthDate == birthDate)
        {
            return;
        }

        person.BirthDate = birthDate;
        logger.LogInformation("{PropertyName} is updated", nameof(Person.LastName));
    }

    private void UpdatePhoneNumbersIfSpecified(Person person, List<PhoneNumberDto> phoneNumbers)
    {
        if (phoneNumbers.IsEmpty())
        {
            return;
        }

        foreach (var phoneNumberDto in phoneNumbers)
        {
            UpdatePhoneNumbers(person, phoneNumberDto);
        }
    }

    private void UpdatePhoneNumbers(Person person, PhoneNumberDto phoneNumberDto)
    {
        var phoneNumberToUpdate = person.PhoneNumbers.Find(phoneNumber => phoneNumber.Type == phoneNumberDto.Type)
                                  ?? throw new PhoneNumberDoesntExistException(phoneNumberDto.Type);

        phoneNumberToUpdate.Number = phoneNumberDto.Number;
        phoneNumberToUpdate.CountryCode = phoneNumberDto.CountryCode;
        phoneNumberToUpdate.AreaCode = phoneNumberDto.AreaCode;

        logger.LogInformation("{PropertyName} of type {Type} is updated", nameof(Person.PhoneNumbers)
            , phoneNumberToUpdate.Type);
    }

    private void UpdatePersonalNumberIfSpecified(Person person, string personalNumber)
    {
        if (personalNumber.IsNullOrEmpty() || person.PersonalNumber == personalNumber)
        {
            return;
        }

        person.PersonalNumber = personalNumber;
        logger.LogInformation("{PropertyName} is updated", nameof(Person.PersonalNumber));
    }

    private void UpdateGenderIfSpecified(Person person, Gender gender)
    {
        if (gender == Gender.NotSpecified || person.Gender == gender)
        {
            return;
        }

        person.Gender = gender;
        logger.LogInformation("{PropertyName} is updated", nameof(Person.Gender));
    }

    private async Task UpdateCityCodeIfSpecifiedAsync(
        Person person,
        string cityCode,
        CancellationToken cancellationToken)
    {
        if (cityCode.IsNullOrEmpty() || person.CityCode == cityCode)
        {
            return;
        }

        await EnsureCityExistsAsync(cityCode, cancellationToken);

        person.CityCode = cityCode;
        logger.LogInformation("{PropertyName} is updated", nameof(Person.CityCode));
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