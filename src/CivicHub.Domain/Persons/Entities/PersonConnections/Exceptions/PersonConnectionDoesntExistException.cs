using CivicHub.Domain.Common.Exceptions;
using CivicHub.Domain.Common.Localization;

namespace CivicHub.Domain.Persons.Entities.PersonConnections.Exceptions;

public class PersonConnectionDoesntExistException : DomainException
{
    public PersonConnectionDoesntExistException(Guid connectionId) : base(
        DomainExceptionMessagesKeys.PersonConnectionDoesntExist, connectionId)
    {
    }
}