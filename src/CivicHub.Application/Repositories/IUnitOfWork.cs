namespace CivicHub.Application.Repositories;

public interface IUnitOfWork
{
    IPersonRepository PersonRepository { get; }
    
    ICityRepository CityRepository { get; }
    
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}