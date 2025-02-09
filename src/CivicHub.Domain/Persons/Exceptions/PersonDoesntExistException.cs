using CivicHub.Domain.Common.Exceptions;

namespace CivicHub.Domain.Persons.Exceptions;

public class PersonDoesntExistException : DomainException
{
    public PersonDoesntExistException(string personalNumber) : base(
        $"Person doesn't exist by personal number {personalNumber}")
    {
    }
    
    public PersonDoesntExistException(long personId) : base(
        $"Person doesn't exist by id {personId}")
    {
    }

    public PersonDoesntExistException(long personId, long otherPersonId) : base(
        $"One of the person or both doesn't exist PersonId: {personId}, OtherPersonId: {otherPersonId}")
    {
    }
}