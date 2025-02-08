using CivicHub.Application.Features.Persons.Queries.GetFullInformation;
using CivicHub.Application.Repositories;
using CivicHub.Domain.Persons;
using CivicHub.Persistance.Contexts.CivicHubContexts;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace CivicHub.Infrastructure.Repositories;

public class PersonRepository(CivicHubContext context) :
    GenericRepository<Person, int>(context),
    IPersonRepository
{
    public async Task<bool> DoesExistAsync(string personalNumber, CancellationToken cancellationToken = default)
        => await Context.Persons.AnyAsync(x => x.PersonalNumber == personalNumber, cancellationToken);

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
            .Where(x => x.Id == personId || x.Id == otherPersonId)
            .ToListAsync(cancellationToken);

        return persons.Count == expectedPersonCount;
    }

    public async Task<GetFullInformationResponse> GetPersonFullInformationAsync(
        long personId,
        CancellationToken cancellationToken = default)
        => await Context.Persons.AsNoTracking()
            .AsSplitQuery()
            .Include(x => x.Connections)
            .ThenInclude(x => x.ConnectedPerson)
            .Include(x => x.ConnectedTo)
            .ThenInclude(x => x.Person)
            .Include(x => x.PhoneNumbers)
            .Include(x => x.City)
            .Where(x => x.Id == personId)
            .ProjectToType<GetFullInformationResponse>()
            .FirstOrDefaultAsync(cancellationToken);
}