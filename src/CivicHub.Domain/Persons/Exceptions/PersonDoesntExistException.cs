using CivicHub.Domain.Common.Exceptions;
using CivicHub.Domain.Common.Localization;

namespace CivicHub.Domain.Persons.Exceptions;

public class PersonDoesntExistException : DomainException
{
    public PersonDoesntExistException(string personalNumber) : base(
        DomainExceptionMessagesKeys.PersonDoesntExistByPersonalNumber, personalNumber)
    {
    }

    public PersonDoesntExistException(long personId) : base(DomainExceptionMessagesKeys.PersonDoesntExistById, personId)
    {
    }

    public PersonDoesntExistException(long personId, long otherPersonId) : base(
        DomainExceptionMessagesKeys.PersonDoesntExistByIdAndOtherPersonId, personId, otherPersonId)
    {
    }
}