using Application.DTO.Booking.AuthorDto;
using Domain.Entities.Booking;
using MediatR;

namespace Application.UseCases.Booking.Commands.AuthorCommands.CreateAuthor;

public record CreateAuthorCommand : IRequest<Author>
{
    public int AuthorId { get; set; }
    public AuthorDto AuthorDto { get; set; }
}