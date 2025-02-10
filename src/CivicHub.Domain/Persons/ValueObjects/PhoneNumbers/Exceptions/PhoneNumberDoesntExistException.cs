using CivicHub.Domain.Common.Exceptions;
using CivicHub.Domain.Common.Localization;
using CivicHub.Domain.Persons.ValueObjects.PhoneNumbers.Enums;

namespace CivicHub.Domain.Persons.ValueObjects.PhoneNumbers.Exceptions;

public class PhoneNumberDoesntExistException : DomainException
{
    public PhoneNumberDoesntExistException(PhoneType phoneType) : base(
        DomainExceptionMessagesKeys.PhoneNumberDoesntExist, phoneType)
    {
    }
}