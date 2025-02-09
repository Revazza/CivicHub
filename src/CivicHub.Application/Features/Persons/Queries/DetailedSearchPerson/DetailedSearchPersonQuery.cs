using CivicHub.Application.Common.Responses;
using CivicHub.Application.Common.Results;
using CivicHub.Application.Features.Persons.Queries.SimpleSearchPerson;
using CivicHub.Domain.Persons.Enums;
using MediatR;

namespace CivicHub.Application.Features.Persons.Queries.DetailedSearchPerson;

public record DetailedSearchPersonQuery(
    int PageSize,
    int PageNumber,
    string FirstName = null,
    string LastName = null,
    string PersonalNumber = null,
    string CityName = null,
    string CityCode = null,
    Gender Gender = Gender.NotSpecified,
    DateTime BirthDateFrom = default,
    DateTime BirthDateTo = default
) : SimpleSearchPersonQuery(PageSize, PageNumber, FirstName, LastName, PersonalNumber);