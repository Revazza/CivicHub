using CivicHub.Domain.Persons.Entities.PersonConnections;

namespace CivicHub.Application.Repositories;

public interface IPersonConnectionRepository : IGenericRepository<PersonConnection, Guid>
{
    Task ExecuteDeleteAsync(long personId, CancellationToken cancellationToken);

    Task<bool> DoesConnectionExistsAsync(
        long personId,
        long connectedPersonId,
        string connectionType,
        CancellationToken cancellationToken = default);
}