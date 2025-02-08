namespace CivicHub.Application.Repositories;

public interface IUnitOfWork
{
    IPersonRepository PersonRepository { get; }
    
    ICityRepository CityRepository { get; }
    
    IPersonConnectionRepository PersonConnectionRepository { get; }
    
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
    
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}