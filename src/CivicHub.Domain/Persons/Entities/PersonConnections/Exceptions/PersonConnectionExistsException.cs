using CivicHub.Domain.Common.Exceptions;
using CivicHub.Domain.Common.Localization;

namespace CivicHub.Domain.Persons.Entities.PersonConnections.Exceptions;

public class PersonConnectionExistsException : DomainException
{
    public PersonConnectionExistsException(string connectionType) : base(
        DomainExceptionMessagesKeys.PersonConnectionAlreadyExists, connectionType)
    {
    }
}