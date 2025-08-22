using Application.Contracts.RepositoryContracts.Booking;
using Application.UseCases.Booking.Commands.BookCommands.DeleteBook;
using Domain.ErrorHandlers;
using FluentAssertions;
using MediatR;
using Moq;

namespace Tests.Booking.Book.Commands;

public class DeleteBook
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly DeleteBookCommandHandler _handler;

    public DeleteBook()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _handler = new DeleteBookCommandHandler(_unitOfWorkMock.Object);
    }
    
    [Fact]
    public async Task Handle_DeleteBookWithCorrectBookId_ReturnsUnitValue()
    {
        // Arrange
        var command = new DeleteBookCommand { BookId = 1 };

        var book = new Domain.Entities.Booking.Book
        {
            Id = 1,
            ISBN = "123",
            BookTitle = "Test Book"
        };

        _unitOfWorkMock.Setup(r => r.BookRepository.GetBookByIdAsync(
                1, 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(book);

        _unitOfWorkMock.Setup(r => r.BookRepository.DeleteAsync(
                book, 
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(Unit.Value);
        _unitOfWorkMock.Verify(unitOfWork => unitOfWork.BookRepository.DeleteAsync(
            book, 
            It.IsAny<CancellationToken>()), 
            Times.Once);
    }

    [Fact]
    public async Task Handle_DeleteBookWithIncorrectBookId_ThrowsNotFoundException()
    {
        // Arrange
        var command = new DeleteBookCommand { BookId = 99 };

        _unitOfWorkMock.Setup(r => r.BookRepository.GetBookByIdAsync(
                99, 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((Domain.Entities.Booking.Book)null!);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage("Book with id 99 was not found");

        _unitOfWorkMock.Verify(unitOfWork => unitOfWork.BookRepository.DeleteAsync(
            It.IsAny<Domain.Entities.Booking.Book>(), 
            It.IsAny<CancellationToken>()), Times.Never);
    }
}