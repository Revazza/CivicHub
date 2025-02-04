namespace CivicHub.Application.Repositories;

public interface IGenericRepository<T, in TId>
    where T : class
{
    Task<T?> InsertAsync(T entity);
    Task<T?> GetByIdAsync(TId id);
    void Update(T entity);
    void Delete(T entity);
}