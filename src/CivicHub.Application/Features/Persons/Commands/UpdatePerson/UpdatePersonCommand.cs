using CivicHub.Application.Common.Results;
using CivicHub.Application.Features.Persons.Commands.Common.Dtos;
using CivicHub.Domain.Persons.Enums;
using MediatR;

namespace CivicHub.Application.Features.Persons.Commands.UpdatePerson;

public record UpdatePersonCommand(
    string FirstName = null,
    string LastName = null,
    string PersonalNumber = null,
    string CityCode = null,
    Gender Gender = Gender.NotSpecified,
    DateTime BirthDate = default,
    List<PhoneNumberDto> PhoneNumbers = null) : IRequest<Result>;