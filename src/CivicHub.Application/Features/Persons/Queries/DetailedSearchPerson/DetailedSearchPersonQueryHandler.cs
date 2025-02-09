using CivicHub.Application.Common.Responses;
using CivicHub.Application.Common.Results;
using CivicHub.Application.Features.Persons.Queries.Common.Builders;
using CivicHub.Application.Repositories;
using MediatR;

namespace CivicHub.Application.Features.Persons.Queries.DetailedSearchPerson;

public class DetailedSearchPersonQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<DetailedSearchPersonQuery, Result<PaginatedResult<ShortPersonResponse>>>
{
    public async Task<Result<PaginatedResult<ShortPersonResponse>>> Handle(
        DetailedSearchPersonQuery request,
        CancellationToken cancellationToken)
    {
        var filter = new PersonExpressionBuilder()
            .AndContainsFirstName(request.FirstName)
            .AndContainsLastName(request.LastName)
            .AndContainsPersonalNumber(request.PersonalNumber)
            .AndEqualToGender(request.Gender)
            .AndIsInBirthDateRange(request.BirthDateFrom, request.BirthDateTo)
            .AndContainsCityName(request.CityName)
            .AndContainsCityCode(request.CityCode)
            .Build();

        var persons = await unitOfWork.PersonRepository.SearchPersonsAsync(
            request.PageNumber,
            request.PageSize,
            filter, cancellationToken);

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