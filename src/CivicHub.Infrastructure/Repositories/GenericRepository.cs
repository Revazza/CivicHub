using CivicHub.Application.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CivicHub.Infrastructure.Repositories;

public class GenericRepository<TEntity, TId>(DbSet<TEntity> dbSet) : IGenericRepository<TEntity, TId>
    where TEntity : class
{
    public async Task InsertAsync(TEntity entity, CancellationToken cancellationToken = default)
        => await dbSet.AddAsync(entity, cancellationToken);

    public async Task<TEntity?> GetByIdAsync(TId id) => await dbSet.FindAsync(id);

    public void Update(TEntity entity)
    {
        dbSet.Update(entity);
    }

    public void Delete(TEntity entity)
    {
        dbSet.Remove(entity);
    }
}