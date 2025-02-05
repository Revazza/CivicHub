using CivicHub.Domain.Common.Exceptions;
using CivicHub.Domain.Persons.ValueObjects.PhoneNumbers.Enums;

namespace CivicHub.Domain.Persons.ValueObjects.PhoneNumbers.Exceptions;

public class PhoneNumberDoesntExistException : DomainException
{
    public PhoneNumberDoesntExistException(PhoneType phoneType) : base($"Phone number by type {phoneType} doesn't exist")
    {
    }
}