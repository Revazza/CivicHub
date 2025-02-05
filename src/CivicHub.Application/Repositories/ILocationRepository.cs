using CivicHub.Domain.Locations;

namespace CivicHub.Application.Repositories;

public interface ILocationRepository : IGenericRepository<Location, Guid>
{
    Task<bool> DoesExistAsync(Guid id, CancellationToken cancellationToken = default);
}