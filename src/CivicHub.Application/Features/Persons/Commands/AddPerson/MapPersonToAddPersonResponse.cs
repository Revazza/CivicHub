using CivicHub.Domain.Persons;
using Mapster;

namespace CivicHub.Application.Features.Persons.Commands.AddPerson;

public class MapPersonToAddPersonResponse : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config
            .NewConfig<Person, AddPersonResponse>()
            .Map(dest => dest.FullName, src => $"{src.FirstName} {src.LastName}");
    }
}