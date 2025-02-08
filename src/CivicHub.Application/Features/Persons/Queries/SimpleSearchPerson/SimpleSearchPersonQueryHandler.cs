using System.Linq.Expressions;
using CivicHub.Application.Common.Extensions;
using CivicHub.Application.Common.Responses;
using CivicHub.Application.Common.Results;
using CivicHub.Application.Repositories;
using CivicHub.Domain.Persons;
using MediatR;

namespace CivicHub.Application.Features.Persons.Queries.SimpleSearchPerson;

public class SimpleSearchPersonQueryHandler(IUnitOfWork unitOfWork) :
    IRequestHandler<SimpleSearchPersonQuery, Result<PaginatedResult<ShortPersonResponse>>>
{
    public async Task<Result<PaginatedResult<ShortPersonResponse>>> Handle(
        SimpleSearchPersonQuery request,
        CancellationToken cancellationToken)
    {
        var persons = await unitOfWork.PersonRepository.SearchPersonsAsync(
            request.PageNumber,
            request.PageSize,
            GetPersonFilter(request), cancellationToken);

        var totalCount = await unitOfWork
            .PersonRepository
            .GetTotalCountAsync(GetPersonFilter(request),cancellationToken);

        return new PaginatedResult<ShortPersonResponse>(
            totalCount,
            request.PageNumber,
            request.PageSize,
            persons
        );
    }

    private static Expression<Func<Person, bool>> GetPersonFilter(SimpleSearchPersonQuery request)
        => person =>
            (request.FirstName.IsNullOrEmpty() || person.FirstName.Contains(request.FirstName)) &&
            (request.LastName.IsNullOrEmpty() || person.LastName.Contains(request.LastName)) &&
            (request.PersonalNumber.IsNullOrEmpty() || person.PersonalNumber.Contains(request.PersonalNumber));
}