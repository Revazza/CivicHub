using CivicHub.Application.Common.Results;
using CivicHub.Application.Repositories;
using CivicHub.Domain.Persons.Exceptions;
using MediatR;

namespace CivicHub.Application.Features.Persons.Queries.GetFullInformation;

public class GetFullInformationQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetFullInformationQuery, Result<GetFullInformationResponse>>
{
    public async Task<Result<GetFullInformationResponse>> Handle(
        GetFullInformationQuery request,
        CancellationToken cancellationToken)
    {
        // Ensuring person exists before executing complex query
        await EnsurePersonExistsAsync(request.PersonId, cancellationToken);
        
        return await unitOfWork
            .PersonRepository
            .GetPersonFullInformationAsync(request.PersonId, cancellationToken);
    }

    private async Task EnsurePersonExistsAsync(long personId, CancellationToken cancellationToken)
    {
        var exists = await unitOfWork.PersonRepository.DoesExistAsync(personId, cancellationToken);
        if (!exists)
        {
            throw new PersonDoesntExistException(personId);
        }
    }
}