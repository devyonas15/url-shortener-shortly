using Application.Contracts.Persistence;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;

namespace Persistence.Repositories;

public class BaseRepository<T> : IBaseRepository<T> where T : class
{
    protected readonly UrlDbContext Context;

    protected BaseRepository(UrlDbContext context)
    {
        Context = context;
    }

    public async Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await Context.Set<T>().AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await Context.Set<T>().FindAsync(new object?[] { id, cancellationToken }, cancellationToken);
    }

    public async Task CreateAsync(T entity, CancellationToken cancellationToken = default)
    {
        await Context.AddAsync(entity, cancellationToken);
        await Context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
    { 
        Context.Remove(entity);
        Context.Entry(entity).State = EntityState.Deleted;
        
        await Context.SaveChangesAsync(cancellationToken);
    }
}