using CivicHub.Application.Common.Results;
using CivicHub.Domain.Persons.Enums;
using MediatR;

namespace CivicHub.Application.Features.Persons.Commands.AddPerson;

public record AddPersonCommand(
    string FirstName,
    string LastName,
    string PersonalNumber,
    Guid LocationId,
    Gender Gender,
    DateTime BirthDate,
    List<PhoneNumberDto> PhoneNumbers) : IRequest<Result<AddPersonResponse>>;