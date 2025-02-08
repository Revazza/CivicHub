using CivicHub.Application.Repositories;
using CivicHub.Persistance.Contexts.CivicHubContexts;
using Microsoft.EntityFrameworkCore.Storage;

namespace CivicHub.Infrastructure.Repositories;

public class UnitOfWork(
    CivicHubContext context,
    Lazy<IPersonRepository> personRepository,
    Lazy<ICityRepository> cityRepository,
    Lazy<IPersonConnectionRepository> personConnectionRepository)
    : IUnitOfWork
{
    private IDbContextTransaction _transaction;

    public IPersonRepository PersonRepository => personRepository.Value;

    public ICityRepository CityRepository => cityRepository.Value;

    public IPersonConnectionRepository PersonConnectionRepository => personConnectionRepository.Value;

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        => _transaction = await context.Database.BeginTransactionAsync(cancellationToken);

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
        => await _transaction.CommitAsync(cancellationToken);

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
        => await _transaction.RollbackAsync(cancellationToken);
    
    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        => await context.SaveChangesAsync(cancellationToken);
}