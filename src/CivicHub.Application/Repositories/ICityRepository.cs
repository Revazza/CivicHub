using CivicHub.Domain.Cities;

namespace CivicHub.Application.Repositories;

public interface ICityRepository : IGenericRepository<City, Guid>
{
    Task<bool> DoesExistAsync(string cityCode, CancellationToken cancellationToken = default);
}