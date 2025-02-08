using CivicHub.Domain.Persons.Enums;

namespace CivicHub.Application.Common.Responses;

public record ShortPersonResponse(long Id, string FullName, string PersonalNumber, Gender Gender, DateTime BirthDate);