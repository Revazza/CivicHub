using CivicHub.Application.Repositories;
using CivicHub.Domain.Persons;
using CivicHub.Persistance.Contexts.CivicHubContexts;
using Microsoft.EntityFrameworkCore;

namespace CivicHub.Infrastructure.Repositories;

public class PersonRepository(CivicHubContext context) :
    GenericRepository<Person, int>(context),
    IPersonRepository
{
    public async Task<bool> DoesExistAsync(string personalNumber, CancellationToken cancellationToken = default)
        => await Context.Persons.AnyAsync(x => x.PersonalNumber == personalNumber, cancellationToken);

    public async Task<Person> GetForUpdateAsync(string personalNumber,
        CancellationToken cancellationToken = default)
        => await Context
            .Persons
            .Include(x => x.PhoneNumbers)
            .FirstOrDefaultAsync(x => x.PersonalNumber == personalNumber, cancellationToken);
}