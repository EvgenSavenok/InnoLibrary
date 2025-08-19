using Application.DTO.Booking.BookDto;
using MediatR;

namespace Application.UseCases.Booking.Commands.BookCommands.CreateBook;

public record CreateBookCommand : IRequest<CreateBookRepsponseDto>
{
    public BookDto BookDto { get; set; }
}