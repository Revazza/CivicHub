using CivicHub.Application.Repositories;
using CivicHub.Domain.Locations;
using CivicHub.Persistance.Contexts.CivicHubContexts;
using Microsoft.EntityFrameworkCore;

namespace CivicHub.Infrastructure.Repositories;

public class LocationRepository(CivicHubContext hubContext) :
    GenericRepository<Location, Guid>(hubContext),
    ILocationRepository
{
    public async Task<bool> DoesExistAsync(Guid id, CancellationToken cancellationToken = default)
        => await _context.Locations.AnyAsync(x => x.Id == id, cancellationToken);
}