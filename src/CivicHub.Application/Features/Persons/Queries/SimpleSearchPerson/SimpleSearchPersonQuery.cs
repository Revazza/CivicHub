using CivicHub.Application.Common.Responses;
using CivicHub.Application.Common.Results;
using MediatR;

namespace CivicHub.Application.Features.Persons.Queries.SimpleSearchPerson;

public record SimpleSearchPersonQuery(
    int PageSize,
    int PageNumber,
    string FirstName = null,
    string LastName = null,
    string PersonalNumber = null) : IRequest<Result<PaginatedResult<ShortPersonResponse>>>;