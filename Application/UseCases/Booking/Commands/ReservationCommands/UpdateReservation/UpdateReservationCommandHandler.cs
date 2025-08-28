using Application.Contracts.Repository.Booking;
using Application.MappingProfiles.Booking;
using Domain.Entities.Booking;
using Domain.ErrorHandlers;
using FluentValidation;
using MediatR;

namespace Application.UseCases.Booking.Commands.ReservationCommands.UpdateReservation;

public class UpdateReservationCommandHandler(
    IUnitOfWork unitOfWork,
    IValidator<UserBookReservation> reservationValidator) : IRequestHandler<UpdateReservationCommand, Unit>
{
    public async Task<Unit> Handle(UpdateReservationCommand request, CancellationToken cancellationToken)
    {
        var reservationId =  request.ReservationId;
        
        var reservationEntity = await unitOfWork.ReservationRepository.GetTrackedReservationByIdAsync(reservationId, cancellationToken);
        if (reservationEntity == null)
        {
            throw new NotFoundException($"Reservation with id {reservationId} not found");
        }
        
        var validationResult = await reservationValidator.ValidateAsync(reservationEntity, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        ReservationMapper.CommandToEntityInUpdate(request, ref reservationEntity);
        
        await unitOfWork.ReservationRepository.UpdateAsync(reservationEntity, cancellationToken);
        
        return Unit.Value;
    }
}