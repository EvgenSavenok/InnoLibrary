using Application.Contracts.RepositoryContracts.Booking;
using Application.UseCases.Booking.Queries.ReservationQueries.GetReservationById;
using Domain.Entities.Booking;
using Domain.ErrorHandlers;
using FluentAssertions;
using Moq;

namespace Tests.Booking.Reservation.Queries;

public class GetReservationById
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly GetReservationByIdQueryHandler _handler;

    public GetReservationById()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _handler = new GetReservationByIdQueryHandler(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_GetReservationByIdWithExistingId_ReturnsReservation()
    {
        // Arrange
        var reservationId = 1;
        var query = new GetReservationByIdQuery(reservationId) { ReservationId = 1 };

        var reservationEntity = new UserBookReservation
        {
            BookId = 1,
            UserId = 1
        };

        _unitOfWorkMock.Setup(unitOfWork => unitOfWork.ReservationRepository.GetReservationByIdAsync(
                query.ReservationId, 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(reservationEntity);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.BookId.Should().Be(1);
        result.UserId.Should().Be(1);

        _unitOfWorkMock.Verify(unitOfWork => unitOfWork.ReservationRepository.GetReservationByIdAsync(
            query.ReservationId, 
            It.IsAny<CancellationToken>()), 
            Times.Once);
    }

    [Fact]
    public async Task Handle_GetReservationByNotExistingId_ThrowsNotFoundException()
    {
        // Arrange
        var reservationId = 99;
        var query = new GetReservationByIdQuery(reservationId) { ReservationId = 99 };

        _unitOfWorkMock.Setup(r => r.ReservationRepository.GetReservationByIdAsync(query.ReservationId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((UserBookReservation)null!);

        // Act
        var act = async () => await _handler.Handle(query, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage("Reservation with id 99 not found.");

        _unitOfWorkMock.Verify(r => r.ReservationRepository.GetReservationByIdAsync(query.ReservationId, It.IsAny<CancellationToken>()), Times.Once);
    }
}