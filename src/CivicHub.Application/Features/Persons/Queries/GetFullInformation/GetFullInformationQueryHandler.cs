using CivicHub.Application.Common.Results;
using CivicHub.Application.Repositories;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CivicHub.Application.Features.Persons.Queries.GetFullInformation;

public class GetFullInformationQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetFullInformationQuery, Result<GetFullInformationResponse>>
{
    public async Task<Result<GetFullInformationResponse>> Handle(
        GetFullInformationQuery request,
        CancellationToken cancellationToken)
        => await unitOfWork
            .PersonRepository
            .AsQueryable()
            .AsNoTracking()
            .AsSplitQuery()
            .Include(x => x.Connections)
            .ThenInclude(x => x.ConnectedPerson)
            .Include(x => x.ConnectedTo)
            .ThenInclude(x => x.Person)
            .Include(x => x.PhoneNumbers)
            .Include(x => x.City)
            .Where(x => x.Id == request.PersonId)
            .ProjectToType<GetFullInformationResponse>()
            .FirstOrDefaultAsync(cancellationToken);

}