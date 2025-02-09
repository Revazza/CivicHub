using CivicHub.Application.Common.Results;
using CivicHub.Application.Repositories;
using MediatR;

namespace CivicHub.Application.Features.Persons.Queries.GetReport;

public class GetReportQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetReportQuery, Result<List<ReportResponse>>>
{
    public async Task<Result<List<ReportResponse>>> Handle(GetReportQuery request, CancellationToken cancellationToken)
    {
        return await unitOfWork.PersonRepository.GetConnectionReportAsync(cancellationToken);
    }
}