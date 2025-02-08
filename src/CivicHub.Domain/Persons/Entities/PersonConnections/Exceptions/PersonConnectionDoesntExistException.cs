using CivicHub.Domain.Common.Exceptions;

namespace CivicHub.Domain.Persons.Entities.PersonConnections.Exceptions;

public class PersonConnectionDoesntExistException : DomainException
{
    public PersonConnectionDoesntExistException(Guid connectionId) : base(
        $"Person connection by id {connectionId} doesn't exist")
    {
    }
}