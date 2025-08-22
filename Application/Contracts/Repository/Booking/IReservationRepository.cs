using Application.RequestFeatures;
using Domain.Entities.Booking;

namespace Application.Contracts.RepositoryContracts.Booking;

public interface IReservationRepository : IBaseRepository<UserBookReservation>
{
    public Task<UserBookReservation?> GetReservationByIdAsync(
        int reservationId, 
        CancellationToken cancellationToken);

    public Task<UserBookReservation?> GetTrackedReservationByIdAsync(
        int reservationId,
        CancellationToken cancellationToken);
    
    public Task<PagedResult<UserBookReservation>> GetAllReservationsAsync(
        ReservationQueryParameters parameters,
        CancellationToken cancellationToken);
}