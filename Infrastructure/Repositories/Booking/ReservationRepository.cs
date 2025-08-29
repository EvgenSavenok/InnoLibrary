using Application.Contracts.Repository.Booking;
using Application.RequestFeatures;
using Domain.Entities.Booking;
using Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Booking;

public class ReservationRepository(BookingContext bookingContext)
    : BaseRepository<UserBookReservation>(bookingContext), IReservationRepository 
{
    public async Task<UserBookReservation?> GetReservationByIdAsync(
        int reservationId, 
        CancellationToken cancellationToken)
    {
        return await bookingContext.UserBookReservations
            .Where(reservation => reservation.Id == reservationId)
            .FirstOrDefaultAsync(cancellationToken);
    }
    
    public async Task<UserBookReservation?> GetTrackedReservationByIdAsync(
        int reservationId,
        CancellationToken cancellationToken)
    {
        return await bookingContext.UserBookReservations
            .Where(reservation => reservation.Id == reservationId)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<PagedResult<UserBookReservation>> GetAllReservationsAsync(
        ReservationQueryParameters parameters, 
        CancellationToken cancellationToken)
    {
        IQueryable<UserBookReservation> query = bookingContext.UserBookReservations.AsNoTracking();
        
        var totalCount = await query.CountAsync(cancellationToken);
        
        query = query.Paging(1, 10);
        
        var items = await query.ToListAsync(cancellationToken);

        return new PagedResult<UserBookReservation>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = parameters.PageNumber,
            PageSize = parameters.PageSize
        };
    }
}