using MediatR;

namespace Application.UseCases.Booking.Commands.BookCommands.DeleteBook;

public record DeleteBookCommand : IRequest<Unit>
{
    public int BookId { get; set; }
}