using Application.RequestFeatures;
using Domain.Entities.Booking;

namespace Application.Contracts.RepositoryContracts.Booking;

public interface IBookRepository : IBaseRepository<Book>
{
    public Task<Book?> GetBookByIdAsync(
        int bookId, 
        CancellationToken cancellationToken);
    
    public Task<PagedResult<Book>> GetAllBooksAsync(
        BookQueryParameters parameters,
        CancellationToken cancellationToken);
    
    public Task<Book?> GetTrackedBookByIdAsync(
        int bookId, 
        CancellationToken cancellationToken);
}