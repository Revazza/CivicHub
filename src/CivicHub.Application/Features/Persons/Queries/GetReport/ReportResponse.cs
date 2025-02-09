namespace CivicHub.Application.Features.Persons.Queries.GetReport;

public record ReportResponse(long PersonId, string FullName, List<PersonConnectionStatistics> Statistics);

public record PersonConnectionStatistics(string ConnectionType, int Count);