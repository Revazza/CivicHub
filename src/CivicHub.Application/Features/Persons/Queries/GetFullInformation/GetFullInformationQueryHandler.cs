using CivicHub.Application.Common.Results;
using CivicHub.Application.Repositories;
using MediatR;

namespace CivicHub.Application.Features.Persons.Queries.GetFullInformation;

public class GetFullInformationQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetFullInformationQuery, Result<GetFullInformationResponse>>
{
    public async Task<Result<GetFullInformationResponse>> Handle(
        GetFullInformationQuery request,
        CancellationToken cancellationToken)
        => await unitOfWork
            .PersonRepository
            .GetPersonFullInformationAsync(request.PersonId, cancellationToken);
}