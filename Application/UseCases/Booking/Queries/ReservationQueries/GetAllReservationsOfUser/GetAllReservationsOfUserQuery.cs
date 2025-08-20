using Application.DTO.Booking.ReservationDto;
using Application.RequestFeatures;
using MediatR;

namespace Application.UseCases.Booking.Queries.ReservationQueries.GetAllReservationsOfUser;

public record GetAllReservationsOfUserQuery : IRequest<PagedResult<ReservationDto>>
{
    public ReservationQueryParameters Parameters { get; set; }
}