using Application.Contracts.RepositoryContracts.Booking;
using Application.DTO.Booking.ReservationDto;
using Application.UseCases.Booking.Commands.ReservationCommands.UpdateReservation;
using Domain.Entities.Booking;
using Domain.ErrorHandlers;
using FluentAssertions;
using MediatR;
using Moq;

namespace Tests.Booking.Reservation.Commands;

public class UpdateReservation
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly UpdateReservationCommandHandler _handler;

    public UpdateReservation()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _handler = new UpdateReservationCommandHandler(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldUpdateReservation_WhenReservationExists()
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

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(Unit.Value);
        _unitOfWorkMock.Verify(unitOfWork => unitOfWork.ReservationRepository.UpdateAsync(
            It.Is<UserBookReservation>(userBookReservation => userBookReservation.DaysBeforeDeadline == command.ReservationDto.DaysBeforeDeadline),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenReservationDoesNotExist()
    {
        // Arrange
        var reservationId = 99;
        var command = new UpdateReservationCommand { ReservationId = reservationId };

        _unitOfWorkMock.Setup(unitOfWork => unitOfWork.ReservationRepository.GetTrackedReservationByIdAsync(
                reservationId, 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((UserBookReservation)null!);

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