using System.Linq.Expressions;
using CivicHub.Application.Common.Responses;
using CivicHub.Application.Features.Persons.Queries.GetFullInformation;
using CivicHub.Application.Features.Persons.Queries.GetReport;
using CivicHub.Domain.Persons;

namespace CivicHub.Application.Repositories;

public interface IPersonRepository : IGenericRepository<Person, long>
{
    Task<bool> DoesExistAsync(string personalNumber, CancellationToken cancellationToken = default);
    
    Task<bool> DoesExistAsync(long personId, CancellationToken cancellationToken = default);
    
    Task<Person> GetForUpdateAsync(string personalNumber, CancellationToken cancellationToken = default);

    Task<Person> GetByPersonalNumberAsync(string personalNumber, CancellationToken cancellationToken = default);

    Task<bool> DoBothPersonsExistAsync(
        long personId,
        long otherPersonId,
        CancellationToken cancellationToken = default);

    Task<GetFullInformationResponse> GetPersonFullInformationAsync(
        long personId,
        CancellationToken cancellationToken = default);

    Task<List<ShortPersonResponse>> SearchPersonsAsync(
        int pageNumber,
        int pageSize,
        Expression<Func<Person, bool>> expression,
        CancellationToken cancellationToken = default);

    Task<int> GetTotalCountAsync(
        Expression<Func<Person, bool>> expression,
        CancellationToken cancellationToken = default);

    Task<List<ReportResponse>> GetConnectionReportAsync(CancellationToken cancellationToken = default);
}