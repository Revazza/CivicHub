using CivicHub.Application.Common.Responses;
using CivicHub.Domain.Persons;
using CivicHub.Domain.Persons.Entities.PersonConnections;
using Mapster;

namespace CivicHub.Application.Features.Persons.Queries.GetFullInformation;

public class Mappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config
            .NewConfig<Person, GetFullInformationResponse>()
            .Map(dest => dest.FullName, src => ConvertToFullName(src.FirstName, src.LastName))
            .Map(dest => dest.City, src => src.City.Adapt<CityResponse>())
            .Map(dest => dest.PhoneNumbers, src => src.PhoneNumbers.Adapt<List<PhoneNumberResponse>>())
            .Map(dest => dest.Connections, src => ConvertToConnectionList(src.Connections, src.ConnectedTo));
        
        config.NewConfig<PersonConnection, PersonConnectionResponse>()
            .Map(dest => dest.Person, src => src.Person.Adapt<ShortPersonResponse>());
    }

    private static string ConvertToFullName(string firstName, string lastName) => $"{firstName} {lastName}";
    
    private static List<PersonConnectionResponse> ConvertToConnectionList(
        List<PersonConnection> connections,
        List<PersonConnection> connectedTo)
    {
        var convertedConnections = connections
            .Select(connection =>
                ConvertToConnectionResponse(connection, connection.ConnectedPerson));
        var convertedConnectedTo = connectedTo
            .Select(connection =>
                ConvertToConnectionResponse(connection, connection.Person));
        return convertedConnections.Concat(convertedConnectedTo).ToList();
    }

    private static PersonConnectionResponse ConvertToConnectionResponse(PersonConnection connection, Person person)
        => new(
            connection.Id,
            connection.ConnectionType,
            person.Adapt<ShortPersonResponse>());

}