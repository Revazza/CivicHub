using CivicHub.Domain.Persons.Entities.PersonConnections;

namespace CivicHub.Application.Repositories;

public interface IPersonConnectionRepository : IGenericRepository<PersonConnection, Guid>
{
    Task<bool> DoesConnectionExistsAsync(
        long personId,
        long connectedPersonId,
        CancellationToken cancellationToken = default);
}