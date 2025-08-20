using Application.DTO.Booking.ReservationDto;
using MediatR;

namespace Application.UseCases.Booking.Queries.ReservationQueries.GetReservationById;

public record GetReservationByIdQuery(int ReservationId) : IRequest<ReservationDto>;