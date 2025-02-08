using CivicHub.Domain.Persons.ValueObjects.PhoneNumbers.Enums;

namespace CivicHub.Application.Common.Responses;

public record PhoneNumberResponse(string FullNumber, PhoneType Type);