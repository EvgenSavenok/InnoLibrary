using Application.Contracts.Repository.Booking;
using Application.UseCases.Booking.Commands.AuthorCommands.DeleteAuthor;
using Domain.ErrorHandlers;
using FluentAssertions;
using MediatR;
using Moq;

namespace Tests.Booking.Author.Commands;

public class DeleteAuthor
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly DeleteAuthorCommandHandler _handler;

    public DeleteAuthor()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();

        _handler = new DeleteAuthorCommandHandler(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_DeleteAuthorWithCorrectId_ReturnsUnitValue()
    {
        // Arrange
        var authorId = 1;
        var command = new DeleteAuthorCommand { AuthorId = authorId };
        var existingAuthor = new Domain.Entities.Booking.Author { AuthorId = authorId, FirstName = "John", LastName = "Doe" };

        _unitOfWorkMock.Setup(unitOfWork => unitOfWork.AuthorRepository.GetAuthorByIdAsync(
                authorId, 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingAuthor);

        _unitOfWorkMock.Setup(unitOfWork => unitOfWork.AuthorRepository.DeleteAsync(
                existingAuthor, 
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(Unit.Value);
        _unitOfWorkMock.Verify(unitOfWork => unitOfWork.AuthorRepository.GetAuthorByIdAsync(
            authorId, 
            It.IsAny<CancellationToken>()), 
            Times.Once);
        _unitOfWorkMock.Verify(unitOfWork => unitOfWork.AuthorRepository.DeleteAsync(
            existingAuthor, 
            It.IsAny<CancellationToken>()), 
            Times.Once);
    }

    [Fact]
    public async Task Handle_CreateAuthorWithIncorrectId_ThrowsNotFoundException()
    {
        // Arrange
        var authorId = 42;
        var command = new DeleteAuthorCommand { AuthorId = authorId };

        _unitOfWorkMock.Setup(unitOfWork => unitOfWork.AuthorRepository.GetAuthorByIdAsync(
                authorId, 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((Domain.Entities.Booking.Author)null!);

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"Author with id {authorId} not found");

        _unitOfWorkMock.Verify(unitOfWork => unitOfWork.AuthorRepository.GetAuthorByIdAsync(
            authorId, 
            It.IsAny<CancellationToken>()), 
            Times.Once);
        _unitOfWorkMock.Verify(unitOfWork => unitOfWork.AuthorRepository.DeleteAsync(
            It.IsAny<Domain.Entities.Booking.Author>(),
            It.IsAny<CancellationToken>()),
            Times.Never);
    }
}