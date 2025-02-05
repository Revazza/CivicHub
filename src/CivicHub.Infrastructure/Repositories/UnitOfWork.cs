using CivicHub.Application.Repositories;
using CivicHub.Persistance.Contexts.CivicHubContexts;

namespace CivicHub.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly CivicHubContext _context;
    private readonly Lazy<IPersonRepository> _personRepository;
    private readonly Lazy<ILocationRepository> _locationRepository;

    public UnitOfWork(
        CivicHubContext context,
        Lazy<IPersonRepository> personRepository,
        Lazy<ILocationRepository> locationRepository)
    {
        _context = context;
        _personRepository = personRepository;
        _locationRepository = locationRepository;
    }

    public IPersonRepository PersonRepository => _personRepository.Value;

    public ILocationRepository LocationRepository => _locationRepository.Value;

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        => await _context.SaveChangesAsync(cancellationToken);
}