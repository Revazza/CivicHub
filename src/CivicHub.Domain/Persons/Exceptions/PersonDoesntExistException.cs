using CivicHub.Domain.Common.Exceptions;

namespace CivicHub.Domain.Persons.Exceptions;

public class PersonDoesntExistException : DomainException
{
    public PersonDoesntExistException(string personalNumber) : base(
        $"Person doesn't exist by personal number {personalNumber}")
    {
    }
}