using Application.Contracts.RepositoryContracts.Booking;
using Application.DTO.Booking.AuthorDto;
using Application.UseCases.Booking.Commands.AuthorCommands.UpdateAuthor;
using Domain.ErrorHandlers;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Moq;

namespace Tests.Booking.Author.Commands;

public class UpdateAuthor
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IValidator<Domain.Entities.Booking.Author>> _validatorMock;
    private readonly UpdateAuthorCommandHandler _handler;

    public UpdateAuthor()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _validatorMock = new Mock<IValidator<Domain.Entities.Booking.Author>>();
        _handler = new UpdateAuthorCommandHandler(_unitOfWorkMock.Object, _validatorMock.Object);
    }

    [Fact]
    public async Task Handle_UpdateAuthorWithCorrectAuthorDtoAndExistingAuthorId_ReturnsUnitValue()
    {
        // Arrange
        var authorId = 1;
        var command = new UpdateAuthorCommand
        {
            AuthorId = authorId,
            AuthorDto = new AuthorDto
            {
                FirstName = "Updated",
                LastName = "Author"
            }
        };

        var existingAuthor = new Domain.Entities.Booking.Author { AuthorId = authorId, FirstName = "Old", LastName = "Name" };

        _unitOfWorkMock.Setup(unitOfWork => unitOfWork.AuthorRepository.GetTrackedAuthorByIdAsync(
                authorId, 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingAuthor);

        _validatorMock.Setup(v => v.ValidateAsync(
                It.IsAny<Domain.Entities.Booking.Author>(), 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _unitOfWorkMock.Setup(unitOfWork => unitOfWork.AuthorRepository.UpdateAsync(
                existingAuthor, 
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(Unit.Value);
        _unitOfWorkMock.Verify(unitOfWork => unitOfWork.AuthorRepository.UpdateAsync(
            It.Is<Domain.Entities.Booking.Author>(author => author.FirstName == "Updated" && author.LastName == "Author"), 
            It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_UpdateAuthorWithNotExistingId_ThrowsNotFoundException()
    {
        // Arrange
        var authorId = 42;
        var command = new UpdateAuthorCommand
        {
            AuthorId = authorId,
            AuthorDto = new AuthorDto
            {
                FirstName = "DoesNot",
                LastName = "Exist"
            }
        };

        _unitOfWorkMock.Setup(r => r.AuthorRepository.GetTrackedAuthorByIdAsync(
                authorId, 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((Domain.Entities.Booking.Author)null!);

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"Author with id {authorId} not found");

        _unitOfWorkMock.Verify(r => r.AuthorRepository.UpdateAsync(
            It.IsAny<Domain.Entities.Booking.Author>(),
            It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_UpdateAuthorWithIncorrectAuthorDto_ThrowsValidationException()
    {
        // Arrange
        var authorId = 5;
        var command = new UpdateAuthorCommand
        {
            AuthorId = authorId,
            AuthorDto = new AuthorDto
            {
                FirstName = "Invalid",
                LastName = "Author"
            }
        };

        var existingAuthor = new Domain.Entities.Booking.Author { AuthorId = authorId, FirstName = "Old", LastName = "Name" };

        _unitOfWorkMock.Setup(r => r.AuthorRepository.GetTrackedAuthorByIdAsync(
                authorId, 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingAuthor);

        var validationResult = new ValidationResult(new[]
        {
            new ValidationFailure("FirstName", "FirstName is required")
        });

        _validatorMock.Setup(v => v.ValidateAsync(existingAuthor, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ValidationException>()
            .WithMessage("*FirstName is required*");

        _unitOfWorkMock.Verify(r => r.AuthorRepository.UpdateAsync(
            It.IsAny<Domain.Entities.Booking.Author>(), 
            It.IsAny<CancellationToken>()),
            Times.Never);
    }
}