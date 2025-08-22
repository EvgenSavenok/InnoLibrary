using System.Linq.Expressions;
using Application.Contracts.RepositoryContracts.Booking;
using Application.UseCases.Booking.Commands.BookCommands.CreateBook;
using FluentAssertions;
using FluentValidation;
using MediatR;
using Moq;

namespace Tests.Booking.Book.Commands;

public class CreateBook
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IValidator<Domain.Entities.Booking.Book>> _validatorMock;
    private readonly CreateBookCommandHandler _handler;

    public CreateBook()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _validatorMock = new Mock<IValidator<Domain.Entities.Booking.Book>>();
        _handler = new CreateBookCommandHandler(_unitOfWorkMock.Object, _validatorMock.Object);
    }
    
    [Fact]
    public async Task Handle_CreateBookWithCorrectBookDto_ReturnsUnitValue()
    {
        // Arrange
        var command = new CreateBookCommand
        {
            BookDto = new Application.DTO.Booking.BookDto.BookDto
            {
                ISBN = "1234567890",
                BookTitle = "Test Book",
                Description = "Some description",
                GenreType = Domain.Enums.Booking.GenreType.Adventures,
                Amount = 10,
                AuthorIds = [1, 2]
            }
        };

        var authors = new List<Domain.Entities.Booking.Author>
        {
            new() { AuthorId = 1, FirstName = "Author 1", LastName = "Author 1" },
            new() { AuthorId = 2, FirstName = "Author 2", LastName = "Author 2" }
        };

        _unitOfWorkMock.Setup(unitOfWork => unitOfWork.AuthorRepository
                .FindByConditionTrackedAsync(
                    It.IsAny<Expression<Func<Domain.Entities.Booking.Author, bool>>>(), 
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(authors);

        _validatorMock.Setup(validator => validator.ValidateAsync(It.IsAny<Domain.Entities.Booking.Book>(), 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());

        _unitOfWorkMock.Setup(unitOfWork => unitOfWork.BookRepository.CreateAsync(
                It.IsAny<Domain.Entities.Booking.Book>(), 
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(Unit.Value);
        _unitOfWorkMock.Verify(unitOfWork => unitOfWork.BookRepository.CreateAsync(
            It.Is<Domain.Entities.Booking.Book>(book => book.BookTitle == "Test Book"), 
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_CreateBookWithIncorrectBookDto_ThrowsValidationException()
    {
        // Arrange
        var command = new CreateBookCommand
        {
            BookDto = new Application.DTO.Booking.BookDto.BookDto
            {
                ISBN = "invalid",
                BookTitle = "",
                Description = "Bad book",
                GenreType = 0,
                Amount = 0,
                AuthorIds = [1]
            }
        };

        var authors = new List<Domain.Entities.Booking.Author>
        {
            new() { AuthorId = 1, FirstName = "Author 1", LastName = "Author 1" }
        };

        _unitOfWorkMock.Setup(r => r.AuthorRepository
                .FindByConditionTrackedAsync(
                    It.IsAny<Expression<Func<Domain.Entities.Booking.Author, bool>>>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(authors);

        _validatorMock.Setup(v => v.ValidateAsync(
                It.IsAny<Domain.Entities.Booking.Book>(), 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult(
                [new FluentValidation.Results.ValidationFailure("BookTitle", "Book title is required")]));

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ValidationException>();
        _unitOfWorkMock.Verify(unitOfWork => unitOfWork.BookRepository.CreateAsync(
            It.IsAny<Domain.Entities.Booking.Book>(), 
            It.IsAny<CancellationToken>()), Times.Never);
    }
}
