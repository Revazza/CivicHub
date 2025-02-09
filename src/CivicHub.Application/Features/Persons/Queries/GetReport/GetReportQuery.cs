using CivicHub.Application.Common.Results;
using MediatR;

namespace CivicHub.Application.Features.Persons.Queries.GetReport;

public record GetReportQuery() : IRequest<Result<List<ReportResponse>>>;