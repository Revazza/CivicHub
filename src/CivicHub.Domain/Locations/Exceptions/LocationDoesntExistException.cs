using CivicHub.Domain.Common.Exceptions;

namespace CivicHub.Domain.Locations.Exceptions;

public class LocationDoesntExistException : DomainException
{
    public LocationDoesntExistException(Guid id) : base($"Location doesn't exist by given id {id}")
    {
    }
}