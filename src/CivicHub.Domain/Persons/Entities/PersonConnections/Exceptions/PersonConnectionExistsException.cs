using CivicHub.Domain.Common.Exceptions;

namespace CivicHub.Domain.Persons.Entities.PersonConnections.Exceptions;

public class PersonConnectionExistsException : DomainException
{
    public PersonConnectionExistsException(long personId, long targetPersonId) : base(
        $"Connection between {personId} and {targetPersonId} already exists")
    {
    }
}