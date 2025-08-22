using System.Linq.Expressions;
using Application.Contracts.RepositoryContracts.Booking;
using Application.UseCases.Booking.Commands.BookCommands.UpdateBook;
using Domain.ErrorHandlers;
using FluentAssertions;
using FluentValidation;
using MediatR;
using Moq;

namespace Tests.Booking.Book.Commands;

public class UpdateBook
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IValidator<Domain.Entities.Booking.Book>> _validatorMock;
    private readonly UpdateBookCommandHandler _handler;

    public UpdateBook()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _validatorMock = new Mock<IValidator<Domain.Entities.Booking.Book>>();
        _handler = new UpdateBookCommandHandler(_unitOfWorkMock.Object, _validatorMock.Object);
    }
    
    [Fact]
    public async Task Handle_UpdateBook_ReturnsUnitValue()
    {
        // Arrange
        var command = new UpdateBookCommand
        {
            BookDto = new Application.DTO.Booking.BookDto.BookDto
            {
                Id = 1,
                ISBN = "1234567890",
                BookTitle = "Updated Title",
                Description = "Updated Desc",
                GenreType = Domain.Enums.Booking.GenreType.Adventures,
                Amount = 15,
                AuthorIds = [1, 2]
            }
        };

        var existingBook = new Domain.Entities.Booking.Book
        {
            Id = 1,
            ISBN = "old",
            BookTitle = "Old Title",
            Description = "Old Desc",
            GenreType = Domain.Enums.Booking.GenreType.FairyTales,
            Amount = 5
        };

        var authors = new List<Domain.Entities.Booking.Author>
        {
            new() { AuthorId = 1, FirstName = "Author 1", LastName = "Author 1" },
            new() { AuthorId = 2, FirstName = "Author 2", LastName = "Author 2" }
        };

        _unitOfWorkMock.Setup(unitOfWork => unitOfWork.BookRepository.GetTrackedBookByIdAsync(
                1, 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingBook);

        _unitOfWorkMock.Setup(unitOfWork => unitOfWork.AuthorRepository.FindByConditionTrackedAsync(
                It.IsAny<Expression<Func<Domain.Entities.Booking.Author, bool>>>(), 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(authors);

        _validatorMock.Setup(validator => validator.ValidateAsync(
                It.IsAny<Domain.Entities.Booking.Book>(), 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());

        _unitOfWorkMock.Setup(r => r.BookRepository.UpdateAsync(
                It.IsAny<Domain.Entities.Booking.Book>(), 
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(Unit.Value);
        _unitOfWorkMock.Verify(unitOfWork => unitOfWork.BookRepository.UpdateAsync(It.Is<Domain.Entities.Booking.Book>(b => 
            b.BookTitle == "Updated Title" && 
            b.Description == "Updated Desc" &&
            b.BookAuthors.Count == 2), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_UpdateBook_ReturnsNotFoundException()
    {
        // Arrange
        var command = new UpdateBookCommand
        {
            BookDto = new Application.DTO.Booking.BookDto.BookDto { Id = 99 }
        };

        _unitOfWorkMock.Setup(unitOfWork => unitOfWork.BookRepository.GetTrackedBookByIdAsync(
                99, 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((Domain.Entities.Booking.Book)null!);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage("Book with id 99 not found.");
        _unitOfWorkMock.Verify(unitOfWork => unitOfWork.BookRepository.UpdateAsync(
            It.IsAny<Domain.Entities.Booking.Book>(), 
            It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_UpdateBook_ThrowsValidationException()
    {
        // Arrange
        var command = new UpdateBookCommand
        {
            BookDto = new Application.DTO.Booking.BookDto.BookDto
            {
                Id = 1,
                ISBN = "invalid",
                BookTitle = "",
                AuthorIds = [1]
            }
        };

        var existingBook = new Domain.Entities.Booking.Book { Id = 1 };

        _unitOfWorkMock.Setup(unitOfWork => unitOfWork.BookRepository.GetTrackedBookByIdAsync(
                1, 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingBook);

        _unitOfWorkMock.Setup(r => r.AuthorRepository.FindByConditionTrackedAsync(
                It.IsAny<Expression<Func<Domain.Entities.Booking.Author, bool>>>(), 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Domain.Entities.Booking.Author>());

        _validatorMock.Setup(v => v.ValidateAsync(
                It.IsAny<Domain.Entities.Booking.Book>(), 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult(
                [new FluentValidation.Results.ValidationFailure("BookTitle", "Title is required")]));

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ValidationException>();
        _unitOfWorkMock.Verify(unitOfWork => unitOfWork.BookRepository.UpdateAsync(
            It.IsAny<Domain.Entities.Booking.Book>(), 
            It.IsAny<CancellationToken>()), Times.Never);
    }
}