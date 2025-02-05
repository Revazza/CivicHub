using CivicHub.Domain.Common.Exceptions;

namespace CivicHub.Domain.Persons.Exceptions;

public class PersonAlreadyExistsException : DomainException
{
    public PersonAlreadyExistsException(string personalNumber) : base($"Person already exists by given personal number {personalNumber}")
    {
    }
}