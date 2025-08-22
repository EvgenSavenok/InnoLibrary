using Application.Contracts.RepositoryContracts.Booking;
using Application.DTO.Booking.AuthorDto;
using Application.UseCases.Booking.Commands.AuthorCommands.CreateAuthor;
using FluentAssertions;
using FluentValidation;
using MediatR;
using Moq;

namespace Tests.Booking.Author.Commands;

public class CreateAuthor
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IValidator<Domain.Entities.Booking.Author>> _validatorMock;
    private readonly CreateAuthorCommandHandler _handler;

    public CreateAuthor()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _validatorMock = new Mock<IValidator<Domain.Entities.Booking.Author>>();
        _handler = new CreateAuthorCommandHandler(_unitOfWorkMock.Object, _validatorMock.Object);
    }
    
    [Fact]
    public async Task Handle_CreateAuthor_ReturnsUnitValue()
    {
        // Arrange
        var command = new CreateAuthorCommand
        {
            AuthorDto = new AuthorDto
            {
                FirstName = "New Author",
                LastName = "New Author"
            }
        };

        _validatorMock.Setup(validator => validator.ValidateAsync(
                It.IsAny<Domain.Entities.Booking.Author>(), 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());

        _unitOfWorkMock.Setup(unitOfWork => unitOfWork.AuthorRepository.CreateAsync(
                It.IsAny<Domain.Entities.Booking.Author>(), 
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(Unit.Value);
        _unitOfWorkMock.Verify(unitOfWork => unitOfWork.AuthorRepository.CreateAsync(
            It.Is<Domain.Entities.Booking.Author>(
                author => author.FirstName == "New Author" && author.LastName == "New Author"), 
            It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_CreateAuthor_ThrowsValidationException()
    {
        // Arrange
        var command = new CreateAuthorCommand
        {
            AuthorDto = new AuthorDto
            {
                FirstName = "New Author",
            }
        };

        _validatorMock.Setup(v => v.ValidateAsync(
                It.IsAny<Domain.Entities.Booking.Author>(), 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult(
                [new FluentValidation.Results.ValidationFailure("Name", "Name is required")]));

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ValidationException>();
        _unitOfWorkMock.Verify(unitOfWork => unitOfWork.AuthorRepository.CreateAsync(
            It.IsAny<Domain.Entities.Booking.Author>(), 
            It.IsAny<CancellationToken>()),
            Times.Never);
    }

}