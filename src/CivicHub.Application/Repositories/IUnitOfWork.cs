namespace CivicHub.Application.Repositories;

public interface IUnitOfWork
{
    IPersonRepository PersonRepository { get; }
    
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}