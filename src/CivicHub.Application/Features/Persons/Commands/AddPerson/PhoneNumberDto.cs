using CivicHub.Domain.Persons.ValueObjects.PhoneNumbers.Enums;

namespace CivicHub.Application.Features.Persons.Commands.AddPerson;

public record PhoneNumberDto(string CountryCode, string AreaCode, string Number, PhoneType Type);