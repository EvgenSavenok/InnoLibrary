using Application.Contracts.Repository.Booking;
using Application.DTO.Booking.BookDto;
using Application.RequestFeatures;
using Application.UseCases.Booking.Queries.BookQueries.GetAllBooks;
using Domain.Enums.Booking;
using FluentAssertions;
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
    
    [Fact]
    public async Task Handle_GetAllBooksFromEmptyTable_ReturnsEmptyResult()
    {
        // Arrange
        var query = new GetAllBooksQuery();

        _repositoryMock.Setup(unitOfWork => unitOfWork.BookRepository.GetAllBooksAsync(
                It.IsAny<BookQueryParameters>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PagedResult<Domain.Entities.Booking.Book>
            {
                Items = new List<Domain.Entities.Booking.Book>(),
                TotalCount = 0,
                PageNumber = 1,
                PageSize = 10
            });

        // Act
        var result = await _handlerTests.Handle(query, CancellationToken.None);

        // Assert
        result.Items.Should().BeEmpty();
        result.TotalCount.Should().Be(0);
    }
    
    [Fact]
    public async Task Handle_GetAllBooksFromFilledTable_ReturnsPaginatedResult()
    {
        // Arrange
        var query = new GetAllBooksQuery();

        var books = new List<Domain.Entities.Booking.Book>
        {
            new()
            {
                Id = 1,
                ISBN = "1234567890",
                BookTitle = "Book 1",
                GenreType = GenreType.LoveStories,
                Description = "Description 1",
                Amount = 5
            },
            new()
            {
                Id = 2,
                ISBN = "0987654321",
                BookTitle = "Book 2",
                GenreType = GenreType.FairyTales,
                Description = "Description 2",
                Amount = 3
            }
        };

        _repositoryMock.Setup(r => r.BookRepository.GetAllBooksAsync(
                It.IsAny<BookQueryParameters>(), 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PagedResult<Domain.Entities.Booking.Book>
            {
                Items = books,
                TotalCount = books.Count,
                PageNumber = 1,
                PageSize = 10
            });

        // Act
        var result = await _handlerTests.Handle(query, CancellationToken.None);

        // Assert
        var items = result.Items.ToList();
        
        items.Should().HaveCount(2);

        items[0].BookTitle.Should().Be("Book 1");
        items[0].GenreType.Should().Be(GenreType.LoveStories);
        items[0].Amount.Should().Be(5);

        items[1].BookTitle.Should().Be("Book 2");
        items[1].GenreType.Should().Be(GenreType.FairyTales);
        items[1].Amount.Should().Be(3);
    }
}