using CivicHub.Application.Repositories;
using CivicHub.Domain.Persons.Entities.PersonConnections;
using CivicHub.Persistance.Contexts.CivicHubContexts;
using Microsoft.EntityFrameworkCore;

namespace CivicHub.Infrastructure.Repositories;

public class PersonConnectionRepository(CivicHubContext context)
    : GenericRepository<PersonConnection, Guid>(context),
        IPersonConnectionRepository
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
        string connectionType,
        CancellationToken cancellationToken = default)
        => await Context.PersonConnections.AnyAsync(connection =>
                ((connection.PersonId == personId && connection.ConnectedPersonId == connectedPersonId) ||
                 (connection.PersonId == connectedPersonId && connection.ConnectedPersonId == personId)) &&
                connection.ConnectionType == connectionType,
            cancellationToken);
}