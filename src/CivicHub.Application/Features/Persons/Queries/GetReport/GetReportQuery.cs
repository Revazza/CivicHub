using CivicHub.Application.Common.Results;
using MediatR;

namespace CivicHub.Application.Features.Persons.Queries.GetReport;

public record GetReportQuery(string ConnectionType) : IRequest<Result<List<ReportResponse>>>;