using Application.Contracts.RepositoryContracts.Booking;
using Application.RequestFeatures;
using Application.UseCases.Booking.Queries.ReservationQueries.GetAllReservationsOfUser;
using Domain.Entities.Booking;
using FluentAssertions;
using Moq;

namespace Tests.Booking.Reservation.Queries;

public class GetAllReservationsOfUser
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly GetAllReservationsOfUserQueryHandler _handler;

        public GetAllReservationsOfUser()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _handler = new GetAllReservationsOfUserQueryHandler(_unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Handle_GetAllReservationsOfUser_ReturnsPaginatedResult()
        {
            // Arrange
            var query = new GetAllReservationsOfUserQuery
            {
                Parameters = new ReservationQueryParameters { PageNumber = 1, PageSize = 10 }
            };

            var reservations = new List<UserBookReservation>
            {
                new() { BookId = 1, UserId = 1 },
                new() { BookId = 2, UserId = 1 }
            };

            var pagedResult = new PagedResult<UserBookReservation>
            {
                Items = reservations,
                TotalCount = 2,
                PageNumber = 1,
                PageSize = 10
            };

            _unitOfWorkMock.Setup(r => r.ReservationRepository.GetAllReservationsAsync(
                    query.Parameters, 
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(pagedResult);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Items.Should().HaveCount(2);
            result.Items.Should().ContainSingle(
                reservationDto => reservationDto.BookId == 1 &&
                                  reservationDto.UserId == 1);
            result.Items.Should().ContainSingle(
                reservationDto => reservationDto.BookId == 2 && 
                                  reservationDto.UserId == 1);

            result.TotalCount.Should().Be(2);
            result.PageNumber.Should().Be(1);
            result.PageSize.Should().Be(10);

            _unitOfWorkMock.Verify(r => r.ReservationRepository.GetAllReservationsAsync(
                query.Parameters, 
                It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_GetAllReservationsOfUser_ReturnsEmptyResult()
        {
            // Arrange
            var query = new GetAllReservationsOfUserQuery
            {
                Parameters = new ReservationQueryParameters { PageNumber = 1, PageSize = 10 }
            };

            var pagedResult = new PagedResult<UserBookReservation>
            {
                Items = new List<UserBookReservation>(),
                TotalCount = 0,
                PageNumber = 1,
                PageSize = 10
            };

            _unitOfWorkMock.Setup(r => r.ReservationRepository.GetAllReservationsAsync(query.Parameters, It.IsAny<CancellationToken>()))
                .ReturnsAsync(pagedResult);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Items.Should().BeEmpty();
            result.TotalCount.Should().Be(0);
            result.PageNumber.Should().Be(1);
            result.PageSize.Should().Be(10);

            _unitOfWorkMock.Verify(r => r.ReservationRepository.GetAllReservationsAsync(query.Parameters, It.IsAny<CancellationToken>()), Times.Once);
        }
}