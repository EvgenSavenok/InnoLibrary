using Domain.Entities.Booking;

namespace Application.Contracts.RepositoryContracts.Booking;

public interface IBookRepository : IBaseRepository<Book>
{
    public Task<Book?> GetBookByIdAsync(int bookId, CancellationToken cancellationToken);
    public Task<IEnumerable<Book>> GetAllBooksAsync(
        CancellationToken cancellationToken,
        int pageNumber = 1,
        int pageSize = 10,
        Func<IQueryable<Book>, IOrderedQueryable<Book>>? orderBy = null);
}