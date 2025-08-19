using Application.Contracts.RepositoryContracts.Booking;
using Application.RequestFeatures;
using Domain.Entities.Booking;
using Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Booking;

public class BookRepository(BookingContext bookingContext)
    : BaseRepository<Book>(bookingContext), IBookRepository
{
    public async Task<Book?> GetBookByIdAsync(int bookId, CancellationToken cancellationToken)
    {
        var books = await FindByConditionAsync(
            book => book.Id == bookId, 
            cancellationToken, 
            book => book.BookReservations,
            book => book.BookReservations);
        
        return books.FirstOrDefault();
    }
    
    public async Task<IEnumerable<Book>> GetAllBooksAsync(
        CancellationToken cancellationToken,
        int pageNumber = 1,
        int pageSize = 10,
        Func<IQueryable<Book>, IOrderedQueryable<Book>>? orderBy = null)
    {
        IQueryable<Book> query = bookingContext.Set<Book>().AsNoTracking();
        
        if (orderBy != null)
            query = orderBy(query);

        query = query.Paging(1, 10);

        return await query.ToListAsync(cancellationToken);
    }
}