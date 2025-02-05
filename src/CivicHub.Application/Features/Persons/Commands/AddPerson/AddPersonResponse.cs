using CivicHub.Domain.Persons.Enums;

namespace CivicHub.Application.Features.Persons.Commands.AddPerson;

public record AddPersonResponse(
    string FullName,
    string PersonalNumber,
    Gender Gender,
    DateTime BirthDate,
    Guid LocationId);