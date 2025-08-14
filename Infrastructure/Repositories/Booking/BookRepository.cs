using Application.Contracts.RepositoryContracts.Booking;
using Domain.Entities.Booking;
using Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Booking;

public class BookRepository(BookingContext repositoryContext)
    : BaseRepository<Book>(repositoryContext), IBookRepository
{
    public async Task<Book?> GetBookByIdAsync(int bookId, CancellationToken cancellationToken)
    {
        var books = await FindByCondition(
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
        var query = FindAllQuery();
        
        if (orderBy != null)
            query = orderBy(query);

        query = query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);

        return await query.ToListAsync(cancellationToken);
    }
}