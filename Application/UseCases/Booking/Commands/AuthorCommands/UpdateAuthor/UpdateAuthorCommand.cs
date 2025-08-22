using Application.DTO.Booking.AuthorDto;
using MediatR;

namespace Application.UseCases.Booking.Commands.AuthorCommands.UpdateAuthor;

public record UpdateAuthorCommand : IRequest<Unit>
{
    public int AuthorId { get; set; }
    public AuthorDto AuthorDto { get; set; }
}