using CivicHub.Application.Repositories;
using CivicHub.Persistance.Contexts.CivicHubContexts;

namespace CivicHub.Infrastructure.Repositories;

public class UnitOfWork(
    CivicHubContext context,
    Lazy<IPersonRepository> personRepository,
    Lazy<ICityRepository> cityRepository,
    Lazy<IPersonConnectionRepository> personConnectionRepository)
    : IUnitOfWork
{
    public IPersonRepository PersonRepository => personRepository.Value;

    public ICityRepository CityRepository => cityRepository.Value;
    
    public IPersonConnectionRepository PersonConnectionRepository => personConnectionRepository.Value;

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        => await context.SaveChangesAsync(cancellationToken);
}