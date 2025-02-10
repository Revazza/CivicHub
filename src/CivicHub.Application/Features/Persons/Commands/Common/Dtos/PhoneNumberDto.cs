using CivicHub.Domain.Persons.ValueObjects.PhoneNumbers.Enums;

namespace CivicHub.Application.Features.Persons.Commands.Common.Dtos;

public record PhoneNumberDto(string Number, PhoneType Type);