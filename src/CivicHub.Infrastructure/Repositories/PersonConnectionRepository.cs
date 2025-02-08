using CivicHub.Application.Repositories;
using CivicHub.Domain.Persons.Entities.PersonConnections;
using CivicHub.Persistance.Contexts.CivicHubContexts;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace CivicHub.Infrastructure.Repositories;

public class PersonConnectionRepository(CivicHubContext context)
    : GenericRepository<PersonConnection, Guid>(context), IPersonConnectionRepository
{
    public async Task ExecuteDeleteAsync(long personId, CancellationToken cancellationToken)
        => await Context
            .PersonConnections
            .Where(b => b.PersonId == personId || b.ConnectedPersonId == personId)
            .ExecuteDeleteAsync(cancellationToken);

    public void Ok()
    {
        var ok = Context.Persons.AsQueryable();
    }
    
    public async Task<bool> DoesConnectionExistsAsync(
        long personId,
        long connectedPersonId,
        CancellationToken cancellationToken = default)
        => await Context.PersonConnections.AnyAsync(x =>
                (x.PersonId == personId && x.ConnectedPersonId == connectedPersonId) ||
                (x.PersonId == connectedPersonId && x.ConnectedPersonId == personId),
            cancellationToken);
}