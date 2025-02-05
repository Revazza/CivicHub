using CivicHub.Application.Common.Results;
using CivicHub.Application.Features.Persons.Commands.Common.Dtos;
using CivicHub.Domain.Persons.Enums;
using MediatR;

namespace CivicHub.Application.Features.Persons.Commands.UpdatePerson;

public record UpdatePersonCommand(
    string FirstName,
    string LastName,
    string PersonalNumber,
    string CityCode,
    Gender Gender,
    DateTime BirthDate,
    List<PhoneNumberDto> PhoneNumbers) : IRequest<Result<UpdatePersonResponse>>;