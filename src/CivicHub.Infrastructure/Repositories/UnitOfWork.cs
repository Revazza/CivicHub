using CivicHub.Application.Repositories;
using CivicHub.Persistance.Contexts.CivicHubContexts;

namespace CivicHub.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly CivicHubContext _context;
    private readonly Lazy<IPersonRepository> _personRepository;
    private readonly Lazy<ICityRepository> _cityRepository;

    public UnitOfWork(
        CivicHubContext context,
        Lazy<IPersonRepository> personRepository,
        Lazy<ICityRepository> cityRepository)
    {
        _context = context;
        _personRepository = personRepository;
        _cityRepository = cityRepository;
    }

    public IPersonRepository PersonRepository => _personRepository.Value;

    public ICityRepository CityRepository => _cityRepository.Value;

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        => await _context.SaveChangesAsync(cancellationToken);
}