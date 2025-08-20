using Application.Contracts.RepositoryContracts.Booking;
using Infrastructure.Contexts;

namespace Infrastructure.Repositories.Booking;

public class UnitOfWork(BookingContext bookingContext) : IUnitOfWork
{
    private IBookRepository _bookRepository;
    private IAuthorRepository _authorRepository;
    private IReservationRepository _reservationRepository;

    public IBookRepository BookRepository
    {
        get
        {
            if (_bookRepository == null)
                _bookRepository = new BookRepository(bookingContext);
            return _bookRepository;
        }
    }
    
    public IAuthorRepository AuthorRepository
    {
        get
        {
            if (_authorRepository == null)
                _authorRepository = new AuthorRepository(bookingContext);
            return _authorRepository;
        }
    }
}