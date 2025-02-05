using CivicHub.Application.Repositories;
using CivicHub.Domain.Cities;
using CivicHub.Persistance.Contexts.CivicHubContexts;
using Microsoft.EntityFrameworkCore;

namespace CivicHub.Infrastructure.Repositories;

public class CityRepository(CivicHubContext hubContext) :
    GenericRepository<City, Guid>(hubContext),
    ICityRepository
{
    public async Task<bool> DoesExistAsync(string cityCode, CancellationToken cancellationToken = default)
        => await _context.Cities.AnyAsync(x => x.Code == cityCode, cancellationToken);
}