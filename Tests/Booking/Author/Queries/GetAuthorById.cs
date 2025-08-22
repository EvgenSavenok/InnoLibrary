using Application.Contracts.RepositoryContracts.Booking;
using Application.UseCases.Booking.Queries.AuthorQueries.GetAuthorByBookId;
using Domain.ErrorHandlers;
using FluentAssertions;
using Moq;

namespace Tests.Booking.Author.Queries;

public class GetAuthorById
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly GetAuthorByIdQueryHandler _handler;

        public GetAuthorById()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _handler = new GetAuthorByIdQueryHandler(_unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Handle_GetAuthorById_ReturnsAuthor()
        {
            // Arrange
            var authorId = 1;
            var query = new GetAuthorByIdQuery(authorId) { AuthorId = authorId };

            var authorEntity = new Domain.Entities.Booking.Author
            {
                AuthorId = 1,
                FirstName = "John",
                LastName = "Doe"
            };

            _unitOfWorkMock.Setup(unitOfWork => unitOfWork.AuthorRepository.GetAuthorByIdAsync(
                    query.AuthorId, 
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(authorEntity);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.FirstName.Should().Be("John");
            result.LastName.Should().Be("Doe");

            _unitOfWorkMock.Verify(unitOfWork => unitOfWork.AuthorRepository.GetAuthorByIdAsync(
                query.AuthorId, 
                It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_GetAuthorById_ThrowsNotFoundException()
        {
            // Arrange
            var authorId = 99;
            var query = new GetAuthorByIdQuery(authorId) { AuthorId = authorId };

            _unitOfWorkMock.Setup(r => r.AuthorRepository.GetAuthorByIdAsync(
                    query.AuthorId, 
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Domain.Entities.Booking.Author)null!);

            // Act
            var act = async () => await _handler.Handle(query, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage("Author with id 99 not found.");

            _unitOfWorkMock.Verify(r => r.AuthorRepository.GetAuthorByIdAsync(
                query.AuthorId, 
                It.IsAny<CancellationToken>()),
                Times.Once);
        }
}