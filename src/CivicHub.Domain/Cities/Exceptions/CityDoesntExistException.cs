using CivicHub.Domain.Common.Exceptions;
using CivicHub.Domain.Common.Localization;

namespace CivicHub.Domain.Cities.Exceptions;

public class CityDoesntExistException : DomainException
{
    public CityDoesntExistException(string cityCode) : base(DomainExceptionMessagesKeys.CityDoesntExist, cityCode)
    {
    }
}