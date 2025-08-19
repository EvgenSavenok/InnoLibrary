namespace Application.Contracts.RepositoryContracts.Booking;

public interface IUnitOfWork
{
    IBookRepository BookRepository { get; }
}