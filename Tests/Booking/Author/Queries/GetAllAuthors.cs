using Application.Contracts.RepositoryContracts.Booking;
using Application.RequestFeatures;
using Application.UseCases.Booking.Queries.AuthorQueries.GetAllAuthors;
using FluentAssertions;
using Moq;

namespace Tests.Booking.Author.Queries;

public class GetAllAuthors
{ 
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly GetAllAuthorsQueryHandler _handler;

    public GetAllAuthors()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _handler = new GetAllAuthorsQueryHandler(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_GetAllAuthorsFromFilledTable_ReturnsPaginatedResult()
    {
        // Arrange
        var query = new GetAllAuthorsQuery
        {
            Parameters = new AuthorQueryParameters { PageNumber = 1, PageSize = 10 }
        };

        var authors = new List<Domain.Entities.Booking.Author>
        {
            new() { AuthorId = 1, FirstName = "John", LastName = "Doe" },
            new() { AuthorId = 2, FirstName = "Jane", LastName = "Smith" }
        };

        var pagedResult = new PagedResult<Domain.Entities.Booking.Author>
        {
            Items = authors,
            TotalCount = 2,
            PageNumber = 1,
            PageSize = 10
        };

        _unitOfWorkMock.Setup(r => r.AuthorRepository.GetAllAuthorsAsync(
                query.Parameters, 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(pagedResult);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(2);
        result.Items.Should().ContainSingle(author => author.FirstName == "John" && author.LastName == "Doe");
        result.Items.Should().ContainSingle(author => author.FirstName == "Jane" && author.LastName == "Smith");

        result.TotalCount.Should().Be(2);
        result.PageNumber.Should().Be(1);
        result.PageSize.Should().Be(10);

        _unitOfWorkMock.Verify(r => r.AuthorRepository.GetAllAuthorsAsync(query.Parameters, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_GetAllAuthorsFromEmptyTable_ReturnsEmptyResult()
    {
        // Arrange
        var query = new GetAllAuthorsQuery
        {
            Parameters = new AuthorQueryParameters { PageNumber = 1, PageSize = 10 }
        };

        var pagedResult = new PagedResult<Domain.Entities.Booking.Author>
        {
            Items = new List<Domain.Entities.Booking.Author>(),
            TotalCount = 0,
            PageNumber = 1,
            PageSize = 10
        };

        _unitOfWorkMock.Setup(r => r.AuthorRepository.GetAllAuthorsAsync(query.Parameters, It.IsAny<CancellationToken>()))
            .ReturnsAsync(pagedResult);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().BeEmpty();
        result.TotalCount.Should().Be(0);
        result.PageNumber.Should().Be(1);
        result.PageSize.Should().Be(10);

        _unitOfWorkMock.Verify(r => r.AuthorRepository.GetAllAuthorsAsync(
            query.Parameters, 
            It.IsAny<CancellationToken>()), 
            Times.Once);
    }
}