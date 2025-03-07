using System.Linq.Expressions;
using CivicHub.Application.Common.Responses;
using CivicHub.Application.Features.Persons.Queries.GetFullInformation;
using CivicHub.Application.Features.Persons.Queries.GetReport;
using CivicHub.Application.Repositories;
using CivicHub.Domain.Persons;
using CivicHub.Persistance.Contexts.CivicHubContexts;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace CivicHub.Infrastructure.Repositories;

public class PersonRepository(CivicHubContext context) :
    GenericRepository<Person, long>(context),
    IPersonRepository
{
    public async Task<bool> DoesExistAsync(string personalNumber, CancellationToken cancellationToken = default)
        => await Context.Persons.AnyAsync(x => x.PersonalNumber == personalNumber, cancellationToken);

    public async Task<bool> DoesExistAsync(long personId, CancellationToken cancellationToken = default)
        => await Context.Persons.AnyAsync(x => x.Id == personId, cancellationToken);

    public async Task<Person> GetForUpdateAsync(
        string personalNumber,
        CancellationToken cancellationToken = default)
        => await Context
            .Persons
            .Include(x => x.PhoneNumbers)
            .FirstOrDefaultAsync(x => x.PersonalNumber == personalNumber, cancellationToken);

    public async Task<Person> GetByPersonalNumberAsync(
        string personalNumber,
        CancellationToken cancellationToken = default)
        => await Context.Persons.FirstOrDefaultAsync(x => x.PersonalNumber == personalNumber, cancellationToken);

    public async Task<bool> DoBothPersonsExistAsync(
        long personId,
        long otherPersonId,
        CancellationToken cancellationToken = default)
    {
        const int expectedPersonCount = 2;

        var persons = await Context
            .Persons
            .AsNoTracking()
            .Where(x => x.Id == personId || x.Id == otherPersonId)
            .ToListAsync(cancellationToken);

        return persons.Count == expectedPersonCount;
    }

    public async Task<GetFullInformationResponse> GetPersonFullInformationAsync(
        long personId,
        CancellationToken cancellationToken = default)
        => await Context.Persons.AsNoTracking()
            .AsSplitQuery()
            .Include(person => person.Connections)
            .ThenInclude(personConnection => personConnection.ConnectedPerson)
            .Include(person => person.ConnectedTo)
            .ThenInclude(personConnection => personConnection.Person)
            .Include(person => person.PhoneNumbers)
            .Include(person => person.City)
            .Where(person => person.Id == personId)
            .ProjectToType<GetFullInformationResponse>()
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<List<ShortPersonResponse>> SearchPersonsAsync(
        int pageNumber,
        int pageSize,
        Expression<Func<Person, bool>> expression,
        CancellationToken cancellationToken = default)
        => await Context
            .Persons
            .AsNoTracking()
            .Where(expression)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ProjectToType<ShortPersonResponse>()
            .ToListAsync(cancellationToken);

    public async Task<int> GetTotalCountAsync(
        Expression<Func<Person, bool>> expression,
        CancellationToken cancellationToken = default)
        => await Context.Persons.AsNoTracking().Where(expression).CountAsync(cancellationToken);

    public async Task<List<ReportResponse>> GetConnectionReportAsync(CancellationToken cancellationToken = default)
        => await Context
            .Persons
            .AsNoTracking()
            .AsSplitQuery()
            .Include(person => person.Connections)
            .Select(person => new ReportResponse(
                person.Id,
                $"{person.FirstName} {person.LastName}",
                person.Connections
                    .Select(x => x.ConnectionType)
                    .GroupBy(connectionType => connectionType)
                    .Select(group => new PersonConnectionStatistics(group.Key, group.Count())).ToList()
            ))
            .ToListAsync(cancellationToken);
}