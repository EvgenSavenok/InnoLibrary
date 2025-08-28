using Application.Contracts.Repository.Booking;
using Application.DTO.Booking.ReservationDto;
using Application.UseCases.Booking.Commands.ReservationCommands.UpdateReservation;
using Domain.Entities.Booking;
using Domain.ErrorHandlers;
using FluentAssertions;
using FluentValidation;
using MediatR;
using Moq;

namespace Tests.Booking.Reservation.Commands;

public class UpdateReservation
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly UpdateReservationCommandHandler _handler;
    private readonly Mock<IValidator<UserBookReservation>> _validatorMock;

    public UpdateReservation()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _validatorMock = new Mock<IValidator<UserBookReservation>>();
        _handler = new UpdateReservationCommandHandler(_unitOfWorkMock.Object,  _validatorMock.Object);
    }
    
    [Fact]
    public async Task Handle_UpdateReservationAndUpdateBookAmount_ReturnsUnitValue()
    {
        // Arrange
        var reservationId = 1;
        var command = new UpdateReservationCommand
        {
            ReservationId = reservationId,
            ReservationDto = new ReservationDto
            {
                BookId = 2, 
                UserId = 3
            }
        };

        var existingReservation = new UserBookReservation
        {
            Id = reservationId,
            BookId = 1,
            UserId = 1
        };

        _unitOfWorkMock.Setup(unitOfWork => unitOfWork.ReservationRepository.GetTrackedReservationByIdAsync(
                reservationId, 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingReservation);

        _unitOfWorkMock.Setup(unitOfWork => unitOfWork.ReservationRepository.UpdateAsync(
                existingReservation, 
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        
        _validatorMock.Setup(validator => validator.ValidateAsync(
                It.IsAny<UserBookReservation>(), 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(Unit.Value);
        _unitOfWorkMock.Verify(unitOfWork => unitOfWork.ReservationRepository.UpdateAsync(
            It.Is<UserBookReservation>(userBookReservation => userBookReservation.DaysBeforeDeadline == command.ReservationDto.DaysBeforeDeadline),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_UpdateReservationAndUpdateBookAmount_ThrowsReservationNotFoundException()
    {
        // Arrange
        var reservationId = 99;
        var command = new UpdateReservationCommand { ReservationId = reservationId };

        _unitOfWorkMock.Setup(unitOfWork => unitOfWork.ReservationRepository.GetTrackedReservationByIdAsync(
                reservationId, 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((UserBookReservation)null!);
        
        _validatorMock.Setup(validator => validator.ValidateAsync(
                It.IsAny<UserBookReservation>(), 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"Reservation with id {reservationId} not found");

        _unitOfWorkMock.Verify(r => r.ReservationRepository.UpdateAsync(
            It.IsAny<UserBookReservation>(), 
            It.IsAny<CancellationToken>()), 
            Times.Never);
    }
}