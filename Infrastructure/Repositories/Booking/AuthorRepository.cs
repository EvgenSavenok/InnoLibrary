using Application.Contracts.Repository.Booking;
using Application.RequestFeatures;
using Domain.Entities.Booking;
using Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Booking;

public class AuthorRepository(BookingContext bookingContext)
    : BaseRepository<Author>(bookingContext), IAuthorRepository
{
    public async Task<Author?> GetAuthorByIdAsync(
        int authorId,
        CancellationToken cancellationToken)
    {
        return await bookingContext.Authors
            .Where(author => author.AuthorId == authorId)
            .FirstOrDefaultAsync(cancellationToken);
    }
    
    public async Task<Author?> GetTrackedAuthorByIdAsync(
        int authorId,
        CancellationToken cancellationToken)
    {
        return await bookingContext.Authors
            .Where(author => author.AuthorId == authorId)
            .FirstOrDefaultAsync(cancellationToken);
    }
    
    public async Task<PagedResult<Author>> GetAllAuthorsAsync(
        AuthorQueryParameters parameters,
        CancellationToken cancellationToken)
    {
        IQueryable<Author> query = bookingContext.Authors.AsNoTracking();

        var totalCount = await query.CountAsync(cancellationToken);

        query = query.Paging(parameters.PageNumber, parameters.PageSize);
        
        var items = await query.ToListAsync(cancellationToken);

        return new PagedResult<Author>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = parameters.PageNumber,
            PageSize = parameters.PageSize
        };
    }
}