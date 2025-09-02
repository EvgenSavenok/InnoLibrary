using Application.Contracts.Repository.Booking;
using Application.RequestFeatures;
using Domain.Entities.Booking;
using Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Booking;

public class BookRepository(BookingContext bookingContext)
    : BaseRepository<Book>(bookingContext), IBookRepository
{
    public async Task<Book?> GetBookByIdAsync(
        int bookId, 
        CancellationToken cancellationToken)
    {
        var books = await FindByConditionAsync(
            book => book.Id == bookId, 
            cancellationToken, 
            book => book.BookAuthors,
            book => book.BookReservations);
        
        return books.FirstOrDefault();
    }
    
    public async Task<PagedResult<Book>> GetAllBooksAsync(
        BookQueryParameters parameters,
        CancellationToken cancellationToken)
    {
        IQueryable<Book> query = bookingContext.Books
            .AsNoTracking()
            .Include(book => book.BookAuthors);

        if (!string.IsNullOrWhiteSpace(parameters.OrderBy))
        {
            query = parameters.Descending
                ? query.OrderByDescending(book => EF.Property<object>(book, parameters.OrderBy))
                : query.OrderBy(book => EF.Property<object>(book, parameters.OrderBy));
        }

        var totalCount = await query.CountAsync(cancellationToken);
        
        query = query.Paging(parameters.PageNumber, parameters.PageSize);
        
        var items = await query.ToListAsync(cancellationToken);

        return new PagedResult<Book>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = parameters.PageNumber,
            PageSize = parameters.PageSize
        };
    }

    public async Task<Book?> GetTrackedBookByIdAsync(
        int bookId,
        CancellationToken cancellationToken)
    {
        var books = await FindByConditionTrackedAsync(
            book => book.Id == bookId,
            cancellationToken,
            book => book.BookAuthors,
            book => book.BookReservations);

        return books.FirstOrDefault();
    }
}