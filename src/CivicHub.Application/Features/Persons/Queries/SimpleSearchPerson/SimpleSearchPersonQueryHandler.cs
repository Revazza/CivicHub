using CivicHub.Application.Common.Responses;
using CivicHub.Application.Common.Results;
using CivicHub.Application.Repositories;
using MediatR;

namespace CivicHub.Application.Features.Persons.Queries.SimpleSearchPerson;

public class SimpleSearchPersonQueryHandler(IUnitOfWork unitOfWork) :
    IRequestHandler<SimpleSearchPersonQuery, Result<List<ShortPersonResponse>>>
{
    public async Task<Result<List<ShortPersonResponse>>> Handle(
        SimpleSearchPersonQuery request,
        CancellationToken cancellationToken)
        => await unitOfWork.PersonRepository.SearchPersonsAsync(
            request.FirstName,
            request.LastName,
            request.PersonalNumber,
            cancellationToken);
}