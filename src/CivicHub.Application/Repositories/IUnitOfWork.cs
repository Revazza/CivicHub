namespace CivicHub.Application.Repositories;

public interface IUnitOfWork
{
    IPersonRepository PersonRepository { get; }
    
    ILocationRepository LocationRepository { get; }
    
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}