using Application.Contracts.RepositoryContracts.Booking;
using Domain.ErrorHandlers;
using MediatR;

namespace Application.UseCases.Booking.Commands.ReservationCommands.DeleteReservation;

public class DeleteReservationCommandHandler(
    IUnitOfWork unitOfWork) : IRequestHandler<DeleteReservationCommand, Unit>
{
    public async Task<Unit> Handle(DeleteReservationCommand request, CancellationToken cancellationToken)
    {
        var reservationId =  request.ReservationId;
        
        var reservationEntity = await unitOfWork.ReservationRepository.GetReservationByIdAsync(
            reservationId, 
            cancellationToken);
        if (reservationEntity == null)
        {
            throw new NotFoundException($"Reservation with id {reservationId} not found");
        }
    
        await unitOfWork.ReservationRepository.DeleteAsync(reservationEntity, cancellationToken);
        
        var bookId = reservationEntity.BookId;
        var bookEntity = await unitOfWork.BookRepository.GetTrackedBookByIdAsync(bookId, cancellationToken);
        if (bookEntity == null)
        {
            throw new NotFoundException($"Book with id {bookId} not found");
        }

        bookEntity.Amount += 1;
        
        await unitOfWork.BookRepository.UpdateAsync(bookEntity, cancellationToken);
        
        return Unit.Value;
    }
}