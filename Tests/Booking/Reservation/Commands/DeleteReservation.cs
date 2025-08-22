using Application.Contracts.RepositoryContracts.Booking;
using Application.UseCases.Booking.Commands.ReservationCommands.DeleteReservation;
using Domain.Entities.Booking;
using Domain.ErrorHandlers;
using FluentAssertions;
using MediatR;
using Moq;

namespace Tests.Booking.Reservation.Commands;

public class DeleteReservation
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly DeleteReservationCommandHandler _handler;

        public DeleteReservation()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _handler = new DeleteReservationCommandHandler(_unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldDeleteReservationAndUpdateBook_WhenReservationAndBookExist()
        {
            // Arrange
            var reservationId = 1;
            var bookEntity = new Domain.Entities.Booking.Book { Id = 1, BookTitle = "Test Book", Amount = 5 };
            var reservationEntity = new UserBookReservation { Id = reservationId, BookId = 1, UserId = 1 };

            _unitOfWorkMock.Setup(unitOfWork => unitOfWork.ReservationRepository.GetReservationByIdAsync(reservationId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(reservationEntity);
            _unitOfWorkMock.Setup(unitOfWork => unitOfWork.BookRepository.GetTrackedBookByIdAsync(bookEntity.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(bookEntity);
            _unitOfWorkMock.Setup(unitOfWork => unitOfWork.ReservationRepository.DeleteAsync(reservationEntity, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(unitOfWork => unitOfWork.BookRepository.UpdateAsync(bookEntity, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(new DeleteReservationCommand { ReservationId = reservationId }, CancellationToken.None);

            // Assert
            result.Should().Be(Unit.Value);
            bookEntity.Amount.Should().Be(6);

            _unitOfWorkMock.Verify(r => r.ReservationRepository.DeleteAsync(reservationEntity, It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(r => r.BookRepository.UpdateAsync(bookEntity, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenReservationDoesNotExist()
        {
            // Arrange
            var reservationId = 99;
            _unitOfWorkMock.Setup(r => r.ReservationRepository.GetReservationByIdAsync(reservationId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((UserBookReservation)null!);

            // Act
            var act = async () => await _handler.Handle(new DeleteReservationCommand { ReservationId = reservationId }, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"Reservation with id {reservationId} not found");

            _unitOfWorkMock.Verify(r => r.ReservationRepository.DeleteAsync(It.IsAny<UserBookReservation>(), It.IsAny<CancellationToken>()), Times.Never);
            _unitOfWorkMock.Verify(r => r.BookRepository.UpdateAsync(It.IsAny<Domain.Entities.Booking.Book>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenBookDoesNotExist()
        {
            // Arrange
            var reservationId = 1;
            var reservationEntity = new UserBookReservation { Id = reservationId, BookId = 99, UserId = 1 };

            _unitOfWorkMock.Setup(r => r.ReservationRepository.GetReservationByIdAsync(reservationId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(reservationEntity);
            _unitOfWorkMock.Setup(r => r.BookRepository.GetTrackedBookByIdAsync(reservationEntity.BookId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Domain.Entities.Booking.Book)null!);

            // Act
            var act = async () => await _handler.Handle(new DeleteReservationCommand { ReservationId = reservationId }, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"Book with id 99 not found");

            _unitOfWorkMock.Verify(r => r.ReservationRepository.DeleteAsync(It.IsAny<UserBookReservation>(), It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(r => r.BookRepository.UpdateAsync(It.IsAny<Domain.Entities.Booking.Book>(), It.IsAny<CancellationToken>()), Times.Never);
        }
}