using Application.Contracts.RepositoryContracts.Booking;
using Application.UseCases.Booking.Queries.BookQueries.GetAllBooks;
using Moq;

namespace Tests.Booking.Book.Queries;

public class GetAllBooks
{
    private readonly Mock<IUnitOfWork> _repositoryMock;
    private readonly GetAllBooksQueryHandler _handlerTests;

    public GetAllBooks()
    {
        _repositoryMock = new Mock<IUnitOfWork>();

        _handlerTests = new GetAllBooksQueryHandler(_repositoryMock.Object);
    }
    
    
}