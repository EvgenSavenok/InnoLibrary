using Application.DTO.Booking.ReservationDto;
using Application.RequestFeatures;
using Application.UseCases.Booking.Commands.ReservationCommands.CreateReservation;
using Application.UseCases.Booking.Commands.ReservationCommands.DeleteReservation;
using Application.UseCases.Booking.Commands.ReservationCommands.UpdateReservation;
using Application.UseCases.Booking.Queries.ReservationQueries.GetAllReservationsOfUser;
using Application.UseCases.Booking.Queries.ReservationQueries.GetReservationById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers.Booking;

[ApiController]
[Route("api/reservations")]
public class ReservationController(
    IMediator mediator)
    : Controller
{
    [HttpGet("getReservationById/{reservationId}")]
    public async Task<IActionResult> GetReservationByIdAsync(int reservationId, CancellationToken cancellationToken)
    {
        var query = new GetReservationByIdQuery(reservationId) { ReservationId = reservationId };
        var reservation =  await mediator.Send(query, cancellationToken);
        
        return Ok(reservation);
    }
    
    [HttpGet("getAllReservations")]
    public async Task<IActionResult> GetAllReservationsAsync(
        [FromQuery] ReservationQueryParameters parameters, 
        CancellationToken cancellationToken)
    {
        var query = new GetAllReservationsOfUserQuery { Parameters = parameters};
        var reservations = await mediator.Send(query, cancellationToken);
        
        return Ok(reservations);
    }
    
    [HttpPost("addReservation")]
    public async Task<IActionResult> CreateReservationAsync(
        [FromBody] ReservationDto reservationDto,
        CancellationToken cancellationToken)
    {
        var command = new CreateReservationCommand
        {
            ReservationDto = reservationDto
        };
        await mediator.Send(command, cancellationToken);
        
        return NoContent();
    }

    [HttpPut("updateReservation/{reservationId}")]
    public async Task<IActionResult> UpdateReservationAsync(
        [FromBody] ReservationDto reservationDto,
        int reservationId,
        CancellationToken cancellationToken)
    {
        var command = new UpdateReservationCommand
        {
            ReservationId = reservationId,
            ReservationDto = reservationDto
        };
        await mediator.Send(command, cancellationToken);
        
        return NoContent();
    }

    [HttpDelete("deleteReservation/{reservationId}")]
    public async Task<IActionResult> DeleteReservationAsync(
        int reservationId,
        CancellationToken cancellationToken)
    {
        var command = new DeleteReservationCommand
        {
            ReservationId = reservationId
        };
        await mediator.Send(command, cancellationToken);
        
        return NoContent();
    }
}