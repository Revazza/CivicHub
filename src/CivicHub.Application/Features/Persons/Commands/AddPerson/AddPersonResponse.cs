namespace CivicHub.Application.Features.Persons.Commands.AddPerson;

public record AddPersonResponse(
    long Id,
    string FullName,
    string PersonalNumber,
    string Gender,
    string BirthDate,
    string CityCode);