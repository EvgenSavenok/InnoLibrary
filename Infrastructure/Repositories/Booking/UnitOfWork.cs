using Application.Contracts.RepositoryContracts.Booking;
using Infrastructure.Contexts;

namespace Infrastructure.Repositories.Booking;

public class UnitOfWork(BookingContext bookingContext) : IUnitOfWork
{
    public IBookRepository BookRepository { get; }
    public IAuthorRepository AuthorRepository { get; }
    public IReservationRepository ReservationRepository { get; }
}