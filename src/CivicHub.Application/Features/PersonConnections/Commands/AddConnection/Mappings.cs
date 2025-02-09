using CivicHub.Domain.Persons.Entities.PersonConnections;
using Mapster;

namespace CivicHub.Application.Features.PersonConnections.Commands.AddConnection;

public class Mappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<AddConnectionCommand, PersonConnection>()
            .Map(dest => dest.Id, src => Guid.NewGuid())
            .Map(dest => dest.ConnectedPersonId, src => src.OtherPersonId);
    }
}