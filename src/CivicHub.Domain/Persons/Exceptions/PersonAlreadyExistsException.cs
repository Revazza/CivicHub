using CivicHub.Domain.Common.Exceptions;
using CivicHub.Domain.Common.Localization;

namespace CivicHub.Domain.Persons.Exceptions;

public class PersonAlreadyExistsException : DomainException
{
    public PersonAlreadyExistsException(string personalNumber) : base(DomainExceptionMessagesKeys.PersonAlreadyExists, personalNumber)
    {
    }
}