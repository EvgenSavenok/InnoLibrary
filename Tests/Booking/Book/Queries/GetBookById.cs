using Application.Contracts.RepositoryContracts.Booking;
using Application.DTO.Booking.BookDto;
using Application.UseCases.Booking.Queries.BookQueries.GetBookById;
using Domain.Enums.Booking;
using Moq;
using FluentAssertions;

namespace Tests.Booking.Book.Queries;

public class GetBookById
{
    private readonly Mock<IUnitOfWork> _repositoryMock;
    private readonly GetBookByIdQueryHandler _handlerTests;
    
    public GetBookById()
    {
        _repositoryMock = new Mock<IUnitOfWork>();
        
        _handlerTests = new GetBookByIdQueryHandler(_repositoryMock.Object);
    }

    [Fact]
    public async Task Handle_GetBookByIdWithExistingBookId_ReturnsBookDto()
    {
        // Arrange
        var bookId = Random.Shared.Next(1, Int32.MaxValue);

        var book = new Domain.Entities.Booking.Book
        {
            UserId = null,
            ISBN = "1111111111111",
            BookTitle = "Title",
            GenreType = GenreType.Adventures,
            Description = "Description",
            Amount = 10,
            BookAuthors = 
            [
                new() { FirstName = "Ivan" },
                new() { LastName = "Ivanov" }
            ],
            BookReservations = []
        };

        var bookDto = new BookDto
        {
            UserId = null,
            ISBN = "1111111111111",
            BookTitle = "Title",
            GenreType = GenreType.Adventures,
            Description = "Description",
            Amount = 10,
            BookAuthors =
            [
                new() { FirstName = "Ivan" },
                new() { LastName = "Ivanov" }
            ],
            BookReservations = []
        };
        
        _repositoryMock.Setup(unitOfWork => unitOfWork.BookRepository.GetBookByIdAsync(
                bookId, 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(book);
        
        var query = new GetBookByIdQuery(bookId);   
        
        // Act 
        var result = await _handlerTests.Handle(query, CancellationToken.None);
        
        // Assert 
        result.Should().BeEquivalentTo(bookDto);
    }
}