using Application.Contracts.Repository.Booking;
using Application.DTO.Booking.ReservationDto;
using Application.MappingProfiles.Booking;
using Domain.ErrorHandlers;
using MediatR;

namespace Application.UseCases.Booking.Queries.ReservationQueries.GetReservationById;

public class GetReservationByIdQueryHandler(
    IUnitOfWork unitOfWork) : IRequestHandler<GetReservationByIdQuery, ReservationDto>
{
    public async Task<ReservationDto> Handle(GetReservationByIdQuery request, CancellationToken cancellationToken)
    {
        int reservationId = request.ReservationId;
        
        var reservationEntity = await unitOfWork.ReservationRepository.GetReservationByIdAsync(
            reservationId, 
            cancellationToken);
        if (reservationEntity == null)
        {
            throw new NotFoundException($"Reservation with id {reservationId} not found.");
        }

        var reservationDto = ReservationMapper.EntityToDto(reservationEntity);
        
        return reservationDto;
    }
}