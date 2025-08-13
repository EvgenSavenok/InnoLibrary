using System.Linq.Expressions;

namespace Application.Contracts.RepositoryContracts.Booking;

public interface IBaseRepository<T>
{
    public Task<IEnumerable<T>> FindAll(CancellationToken cancellationToken);
    
    public Task<IEnumerable<T>> FindByCondition(
        Expression<Func<T, bool>> expression,
        CancellationToken cancellationToken,
        params Expression<Func<T, object>>[] includes);
    
    Task Create(T entity, CancellationToken cancellationToken);
    
    Task Update(T entity, CancellationToken cancellationToken);
    
    Task Delete(T entity, CancellationToken cancellationToken);
}