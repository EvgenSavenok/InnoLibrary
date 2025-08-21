using Application.Contracts.RepositoryContracts.Booking;
using Application.MappingProfiles.Booking;
using Domain.ErrorHandlers;
using MediatR;

namespace Application.UseCases.Booking.Commands.ReservationCommands.UpdateReservation;

public class UpdateReservationCommandHandler(
    IUnitOfWork unitOfWork) : IRequestHandler<UpdateReservationCommand, Unit>
{
    public async Task<Unit> Handle(UpdateReservationCommand request, CancellationToken cancellationToken)
    {
        var reservationId =  request.ReservationId;
        
        var reservationEntity = await unitOfWork.ReservationRepository.GetTrackedReservationByIdAsync(reservationId, cancellationToken);
        if (reservationEntity == null)
        {
            throw new NotFoundException($"Reservation with id {reservationId} not found");
        }

        ReservationMapper.CommandToEntityInUpdate(request, ref reservationEntity);
        
        await unitOfWork.ReservationRepository.UpdateAsync(reservationEntity, cancellationToken);
        
        return Unit.Value;
    }
}