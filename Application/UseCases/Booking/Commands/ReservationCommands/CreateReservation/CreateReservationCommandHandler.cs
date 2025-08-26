using Application.Contracts.RepositoryContracts.Booking;
using Application.MappingProfiles.Booking;
using Domain.Entities.Booking;
using Domain.ErrorHandlers;
using FluentValidation;
using MediatR;

namespace Application.UseCases.Booking.Commands.ReservationCommands.CreateReservation;

public class CreateReservationCommandHandler(
    IUnitOfWork unitOfWork,
    IValidator<UserBookReservation> reservationValidator) : IRequestHandler<CreateReservationCommand, Unit>
{
    public async Task<Unit> Handle(CreateReservationCommand request, CancellationToken cancellationToken)
    {
        var reservationEntity = ReservationMapper.DtoToEntity(request.ReservationDto);
        
        var validationResult = await reservationValidator.ValidateAsync(reservationEntity, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }
     
        await unitOfWork.ReservationRepository.CreateAsync(reservationEntity, cancellationToken);
        
        var bookId = request.ReservationDto.BookId;
        var bookEntity = await unitOfWork.BookRepository.GetTrackedBookByIdAsync(bookId, cancellationToken);
        if (bookEntity == null)
        {
            throw new NotFoundException($"Book with {bookId} not found");
        }
        
        bookEntity.Amount -= 1;

        if (bookEntity.Amount > 0)
        {
            await unitOfWork.BookRepository.UpdateAsync(bookEntity, cancellationToken);
        }
        
        return Unit.Value;
    }
}