using CivicHub.Application.Common.Responses;
using CivicHub.Domain.Cities;
using CivicHub.Domain.Persons;
using CivicHub.Domain.Persons.ValueObjects.PhoneNumbers;
using Mapster;

namespace CivicHub.Application.Common.Mappings;

public class GeneralMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        // If it becomes large we'll split it to each mapping class
        
        config.NewConfig<Person, ShortPersonResponse>()
            .Map(dest => dest.FullName, src => ConvertToFullName(src.FirstName, src.LastName));
        
        config.NewConfig<City, CityResponse>();

        config.NewConfig<PhoneNumber, PhoneNumberResponse>()
            .Map(dest => dest.FullNumber, src => ConvertToFullPhoneNumber(src));
    }
    
    private static string ConvertToFullName(string firstName, string lastName) => $"{firstName} {lastName}";
    
    private static string ConvertToFullPhoneNumber(PhoneNumber phoneNumber)
        => $"+({phoneNumber.CountryCode}) {phoneNumber.AreaCode} {phoneNumber.Number}";
}