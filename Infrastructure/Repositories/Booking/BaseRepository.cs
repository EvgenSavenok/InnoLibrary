using System.Linq.Expressions;
using Application.Contracts.Repository.Booking;
using Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Booking;

public abstract class BaseRepository<T>(BookingContext bookingContext) : IBaseRepository<T> 
    where T : class
{
    public async Task<IEnumerable<T>> FindByConditionAsync(
        Expression<Func<T, bool>> expression,
        CancellationToken cancellationToken,
        params Expression<Func<T, object>>[] includes)
    {
        IQueryable<T> query = bookingContext.Set<T>();
        
        query = query.Where(expression).AsNoTracking();

        if (includes is { Length: > 0 })
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }
        
        return await query.ToListAsync(cancellationToken);
    }
    
    public async Task<IEnumerable<T>> FindByConditionTrackedAsync(
        Expression<Func<T, bool>> expression,
        CancellationToken cancellationToken,
        params Expression<Func<T, object>>[] includes)
    {
        IQueryable<T> query = bookingContext.Set<T>().Where(expression);

        if (includes is { Length: > 0 })
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }

        return await query.ToListAsync(cancellationToken);
    }

    public async Task CreateAsync(T entity, CancellationToken cancellationToken = default)
    {
        await bookingContext.Set<T>().AddAsync(entity, cancellationToken); 
        await bookingContext.SaveChangesAsync(cancellationToken);
    }
    
    public async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        bookingContext.Set<T>().Update(entity); 
        await bookingContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
    {
        bookingContext.Set<T>().Remove(entity);
        await bookingContext.SaveChangesAsync(cancellationToken);
    }
}