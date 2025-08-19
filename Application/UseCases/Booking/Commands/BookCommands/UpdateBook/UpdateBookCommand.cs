using Application.DTO.Booking.BookDto;
using MediatR;

namespace Application.UseCases.Booking.Commands.BookCommands.UpdateBook;

public record UpdateBookCommand : IRequest<Unit>
{
    public BookDto BookDto { get; set; } 
}