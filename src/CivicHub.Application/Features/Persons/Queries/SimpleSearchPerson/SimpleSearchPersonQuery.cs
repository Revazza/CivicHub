using CivicHub.Application.Common.Responses;
using CivicHub.Application.Common.Results;
using MediatR;

namespace CivicHub.Application.Features.Persons.Queries.SimpleSearchPerson;

public record SimpleSearchPersonQuery(
    string FirstName,
    string LastName,
    string PersonalNumber,
    int PageSize,
    int PageNumber) : IRequest<Result<PaginatedResult<ShortPersonResponse>>>;