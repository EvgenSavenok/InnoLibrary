using Application.Contracts.RepositoryContracts.Booking;
using Application.DTO.Booking.ReservationDto;
using Application.UseCases.Booking.Commands.ReservationCommands.CreateReservation;
using Domain.Entities.Booking;
using Domain.ErrorHandlers;
using FluentAssertions;
using MediatR;
using Moq;

namespace Tests.Booking.Reservation.Commands;

public class CreateReservation
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly CreateReservationCommandHandler _handler;

        public CreateReservation()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _handler = new CreateReservationCommandHandler(_unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldCreateReservationAndUpdateBook_WhenBookExistsAndAmountPositive()
        {
            // Arrange
            var reservationDto = new ReservationDto { BookId = 1, UserId = 1 };
            var command = new CreateReservationCommand { ReservationDto = reservationDto };

            var bookEntity = new Domain.Entities.Booking.Book { Id = 1, BookTitle = "Test Book", Amount = 5 };

            _unitOfWorkMock.Setup(unitOfWork => unitOfWork.BookRepository.GetTrackedBookByIdAsync(
                    bookEntity.Id, 
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(bookEntity);

            _unitOfWorkMock.Setup(unitOfWork => unitOfWork.ReservationRepository.CreateAsync(
                    It.IsAny<UserBookReservation>(), 
                    It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _unitOfWorkMock.Setup(unitOfWork => unitOfWork.BookRepository.UpdateAsync(
                    It.IsAny<Domain.Entities.Booking.Book>(), 
                    It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be(Unit.Value);
            _unitOfWorkMock.Verify(unitOfWork => unitOfWork.ReservationRepository.CreateAsync(It.Is<UserBookReservation>(
                userBookReservation => userBookReservation.BookId == 1 && userBookReservation.UserId == 1), 
                It.IsAny<CancellationToken>()), 
                Times.Once);

            _unitOfWorkMock.Verify(unitOfWork => unitOfWork.BookRepository.UpdateAsync(It.Is<Domain.Entities.Booking.Book>(
                book => book.Amount == 4), 
                It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenBookDoesNotExist()
        {
            // Arrange
            var reservationDto = new ReservationDto { BookId = 99, UserId = 1 };
            var command = new CreateReservationCommand { ReservationDto = reservationDto };

            _unitOfWorkMock.Setup(r => r.BookRepository.GetTrackedBookByIdAsync(
                    reservationDto.BookId, 
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Domain.Entities.Booking.Book)null!);

            _unitOfWorkMock.Setup(r => r.ReservationRepository.CreateAsync(
                    It.IsAny<UserBookReservation>(), 
                    It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage("Book with 99 not found");

            _unitOfWorkMock.Verify(r => r.BookRepository.UpdateAsync(
                It.IsAny<Domain.Entities.Booking.Book>(), 
                It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldNotUpdateBook_WhenAmountAfterDecrementIsZeroOrNegative()
        {
            // Arrange
            var reservationDto = new ReservationDto {BookId = 1, UserId = 1 };
            var command = new CreateReservationCommand { ReservationDto = reservationDto };

            var bookEntity = new Domain.Entities.Booking.Book { Id = 1, BookTitle = "Test Book", Amount = 1 };

            _unitOfWorkMock.Setup(unitOfWork => unitOfWork.BookRepository.GetTrackedBookByIdAsync(
                    bookEntity.Id, 
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(bookEntity);

            _unitOfWorkMock.Setup(unitOfWork => unitOfWork.ReservationRepository.CreateAsync(
                    It.IsAny<UserBookReservation>(), 
                    It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be(Unit.Value);
            bookEntity.Amount.Should().Be(0); 
            _unitOfWorkMock.Verify(unitOfWork => unitOfWork.BookRepository.UpdateAsync(
                It.IsAny<Domain.Entities.Booking.Book>(), 
                It.IsAny<CancellationToken>()), Times.Never);
        }
}