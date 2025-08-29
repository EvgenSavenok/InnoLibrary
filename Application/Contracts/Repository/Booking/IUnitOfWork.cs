namespace Application.Contracts.Repository.Booking;

public interface IUnitOfWork
{
    IBookRepository BookRepository { get; }
    IAuthorRepository AuthorRepository { get; }
    IReservationRepository ReservationRepository { get; }
}