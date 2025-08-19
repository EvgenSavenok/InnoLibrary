using System.Linq.Expressions;

namespace Application.Contracts.RepositoryContracts.Booking;

public interface IBaseRepository<T>
{
    public Task<IEnumerable<T>> FindByConditionAsync(
        Expression<Func<T, bool>> expression,
        CancellationToken cancellationToken,
        params Expression<Func<T, object>>[] includes);

    public Task<IEnumerable<T>> FindByConditionTracked(
        Expression<Func<T, bool>> expression,
        CancellationToken cancellationToken,
        params Expression<Func<T, object>>[] includes);
    
    Task CreateAsync(T entity, CancellationToken cancellationToken);
    
    Task UpdateAsync(T entity, CancellationToken cancellationToken);
    
    Task Delete(T entity, CancellationToken cancellationToken);
}