using CivicHub.Application.Common.Results;
using CivicHub.Domain.Persons.Enums;
using MediatR;

namespace CivicHub.Application.Features.Persons.Commands.AddPerson;

public record AddPersonCommand(
    string FirstName,
    string LastName,
    string PersonalNumber,
    string CityCode,
    Gender Gender,
    DateTime BirthDate
    ) : IRequest<Result<AddPersonResponse>>;