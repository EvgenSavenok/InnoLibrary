using Application.DTO.Booking.ReservationDto;
using MediatR;

namespace Application.UseCases.Booking.Commands.ReservationCommands.CreateReservation;

public record CreateReservationCommand : IRequest<Unit>
{
    public ReservationDto ReservationDto { get; set; }
}