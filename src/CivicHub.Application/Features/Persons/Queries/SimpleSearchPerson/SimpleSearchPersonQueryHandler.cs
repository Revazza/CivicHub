using CivicHub.Application.Common.Responses;
using CivicHub.Application.Common.Results;
using CivicHub.Application.Features.Persons.Queries.Common.Builders;
using CivicHub.Application.Repositories;
using MediatR;

namespace CivicHub.Application.Features.Persons.Queries.SimpleSearchPerson;

public class SimpleSearchPersonQueryHandler(IUnitOfWork unitOfWork) :
    IRequestHandler<SimpleSearchPersonQuery, Result<PaginatedResult<ShortPersonResponse>>>
{
    public async Task<Result<PaginatedResult<ShortPersonResponse>>> Handle(
        SimpleSearchPersonQuery request,
        CancellationToken cancellationToken)
    {
        var filter = new PersonExpressionBuilder()
            .AndContainsFirstName(request.FirstName)
            .AndContainsLastName(request.LastName)
            .AndContainsPersonalNumber(request.PersonalNumber)
            .Build();

        var persons = await unitOfWork.PersonRepository.SearchPersonsAsync(
            request.PageNumber,
            request.PageSize,
            filter,
            cancellationToken);

        var totalCount = await unitOfWork
            .PersonRepository
            .GetTotalCountAsync(filter, cancellationToken);

        return new PaginatedResult<ShortPersonResponse>(
            totalCount,
            request.PageNumber,
            request.PageSize,
            persons
        );
    }
}