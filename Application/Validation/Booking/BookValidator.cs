using Domain.Entities.Booking;
using FluentValidation;

namespace Application.Validation.Booking;

public class BookValidator : AbstractValidator<Book>
{
    public BookValidator()
    {
        RuleFor(book => book.ISBN)
            .NotEmpty().WithMessage("Book ISBN must not be empty")
            .Must(book => book.Length == 13).WithMessage("Book ISBN must be 13 characters long");
        
        RuleFor(book => book.BookTitle)
            .NotEmpty().WithMessage("Book title must not be empty")
            .MaximumLength(255).WithMessage("Book title must not exceed 255 characters");

        RuleFor(book => book.Amount)
            .GreaterThanOrEqualTo((Int16)0).WithMessage("Book amount must not be negative")
            .LessThanOrEqualTo(Int16.MaxValue).WithMessage("Book amount must be less than or equal to 32,767");
        
        RuleFor(book => book.Description)
            .NotEmpty().WithMessage("Book description must not be empty")
            .MaximumLength(1000).WithMessage("Book description must be not exceed 1000 characters long");

        RuleFor(book => book.GenreType)
            .NotEmpty().WithMessage("Book genre must not be empty");
    }
}