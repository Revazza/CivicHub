using CivicHub.Application.Common.Extensions;
using CivicHub.Domain.Persons;
using CivicHub.Domain.Persons.ValueObjects.PhoneNumbers;
using Mapster;

namespace CivicHub.Application.Features.Persons.Commands.AddPerson;

public class Mappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config
            .NewConfig<AddPersonCommand, Person>()
            .Map(dest => dest.FirstName, src => src.FirstName.ToLower().Capitalize())
            .Map(dest => dest.LastName, src => src.LastName.ToLower().Capitalize())
            .Map(dest => dest.PhoneNumbers, src => src.PhoneNumbers.Adapt<List<PhoneNumber>>());
    }
}