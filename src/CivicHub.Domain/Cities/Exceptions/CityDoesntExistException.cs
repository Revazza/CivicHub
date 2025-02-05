using CivicHub.Domain.Common.Exceptions;

namespace CivicHub.Domain.Cities.Exceptions;

public class CityDoesntExistException : DomainException
{
    public CityDoesntExistException(string cityCode) : base($"City doesn't exist by given code {cityCode}")
    {
    }
}