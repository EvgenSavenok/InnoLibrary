using Application.DTO.Booking.ReservationDto;
using MediatR;

namespace Application.UseCases.Booking.Commands.ReservationCommands.UpdateReservation;

public record UpdateReservationCommand : IRequest<Unit>
{
   public int ReservationId { get; set; }
   public ReservationDto ReservationDto { get; set; }
}