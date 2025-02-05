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
            .NewConfig<Person, AddPersonResponse>()
            .Map(dest => dest.FullName, src => $"{src.FirstName} {src.LastName}")
            .Map(dest => dest.BirthDate, src => src.BirthDate.ToLongDateString())
            .Map(dest => dest.Gender, src => src.Gender.ToString());

        config
            .NewConfig<AddPersonCommand, Person>()
            .Map(dest => dest.FirstName, src => src.FirstName.ToLower().Capitalize())
            .Map(dest => dest.LastName, src => src.LastName.ToLower().Capitalize())
            .Map(dest => dest.PhoneNumbers, src => src.PhoneNumbers.Adapt<List<PhoneNumber>>());
    }
}