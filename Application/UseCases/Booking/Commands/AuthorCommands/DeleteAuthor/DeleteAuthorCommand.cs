using MediatR;

namespace Application.UseCases.Booking.Commands.AuthorCommands.DeleteAuthor;

public record DeleteAuthorCommand : IRequest<Unit>
{
    public int AuthorId { get; set; }
}