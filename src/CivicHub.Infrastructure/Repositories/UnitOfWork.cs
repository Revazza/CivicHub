using CivicHub.Application.Repositories;
using CivicHub.Persistance.Contexts.CivicHubContexts;

namespace CivicHub.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly CivicHubContext _context;
    private readonly Lazy<IPersonRepository> _personRepository;

    public UnitOfWork(CivicHubContext context, Lazy<IPersonRepository> personRepository)
    {
        _context = context;
        _personRepository = personRepository;
    }

    public IPersonRepository PersonRepository => _personRepository.Value;

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        => await _context.SaveChangesAsync(cancellationToken);
}