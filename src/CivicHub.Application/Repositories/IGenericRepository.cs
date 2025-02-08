namespace CivicHub.Application.Repositories;

public interface IGenericRepository<TEntity, in TId>
    where TEntity : class
{
    Task InsertAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task<TEntity> GetByIdAsync(TId id);
    void Update(TEntity entity);
    void Delete(TEntity entity);
}