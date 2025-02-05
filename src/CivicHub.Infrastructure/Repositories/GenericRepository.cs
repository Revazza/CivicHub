using CivicHub.Application.Repositories;
using CivicHub.Persistance.Contexts.CivicHubContexts;
using Microsoft.EntityFrameworkCore;

namespace CivicHub.Infrastructure.Repositories;

public abstract class GenericRepository<TEntity, TId> : IGenericRepository<TEntity, TId>
    where TEntity : class
{
    private readonly DbSet<TEntity> _dbSet;
    protected readonly CivicHubContext _context;

    protected GenericRepository(CivicHubContext context)
    {
        _context = context;
        _dbSet = context.Set<TEntity>();
    }
    
    public async Task InsertAsync(TEntity entity, CancellationToken cancellationToken = default)
        => await _dbSet.AddAsync(entity, cancellationToken);

    public async Task<TEntity> GetByIdAsync(TId id) => await _dbSet.FindAsync(id);

    public void Update(TEntity entity)
    {
        _dbSet.Update(entity);
    }

    public void Delete(TEntity entity)
    {
        _dbSet.Remove(entity);
    }
}