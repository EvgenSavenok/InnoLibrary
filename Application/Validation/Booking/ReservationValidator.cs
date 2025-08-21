using Domain.Entities.Booking;
using FluentValidation;

namespace Application.Validation.Booking;

public class ReservationValidator : AbstractValidator<UserBookReservation>
{
    public ReservationValidator()
    {
        RuleFor(reservation => reservation.BookId)
            .NotEmpty().WithMessage("Book ID must not be empty");
        
        RuleFor(reservation => reservation.DaysBeforeDeadline)
            .NotEmpty().WithMessage("Days before deadline must not be empty")
            .GreaterThanOrEqualTo(-1).WithMessage("Days before deadline must be greater or equal to zero");
        
        RuleFor(reservation => reservation.ReservationDate)
            .NotEmpty().WithMessage("Reservation date must not be empty")
            .Must(BeAValidDate).WithMessage("Reservation date must be valid");
        
        RuleFor(reservation => reservation.ReturnDate)
            .NotEmpty().WithMessage("Return date must not be empty")
            .Must(BeAValidDate).WithMessage("Return date must be valid")
            .Must(BeGreaterThanReservationDate)
            .WithMessage("Return date must be greater than reservation date");
    }
    
    private bool BeAValidDate(DateTime? date)
    {
        return date.HasValue && date.Value != default;
    }
    
    private bool BeGreaterThanReservationDate(UserBookReservation reservation, DateTime? returnDate)
    {
        if (!reservation.ReservationDate.HasValue || !returnDate.HasValue)
            return false;

        return returnDate.Value > reservation.ReservationDate.Value;
    }
}