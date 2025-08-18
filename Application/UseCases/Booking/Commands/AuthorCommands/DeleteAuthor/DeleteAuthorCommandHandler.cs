using Application.Contracts.RepositoryContracts.Booking;
using Domain.ErrorHandlers;
using MediatR;

namespace Application.UseCases.Booking.Commands.AuthorCommands.DeleteAuthor;

public class DeleteAuthorCommandHandler(
    IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteAuthorCommand, Unit>
{
    public async Task<Unit> Handle(DeleteAuthorCommand request, CancellationToken cancellationToken)
    {
        var authorId =  request.AuthorId;
        
        var authorEntity = await unitOfWork.AuthorRepository.GetAuthorByIdAsync(authorId, cancellationToken);
        if (authorEntity == null)
        {
            throw new NotFoundException($"Author with id {authorId} not found");
        }

        await unitOfWork.AuthorRepository.Delete(authorEntity, cancellationToken);
        
        return Unit.Value;
    }
}