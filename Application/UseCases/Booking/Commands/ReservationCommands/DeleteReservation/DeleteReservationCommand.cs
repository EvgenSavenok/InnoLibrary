using MediatR;

namespace Application.UseCases.Booking.Commands.ReservationCommands.DeleteReservation;

public record DeleteReservationCommand :  IRequest<Unit>
{
    public int ReservationId { get; set; }
}