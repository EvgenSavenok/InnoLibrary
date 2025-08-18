using Application.Contracts.RepositoryContracts.Booking;
using Application.MappingProfiles.Booking;
using Domain.ErrorHandlers;
using MediatR;

namespace Application.UseCases.Booking.Commands.AuthorCommands.UpdateAuthor;

public class UpdateAuthorCommandHandler(
    IUnitOfWork unitOfWork) 
    :  IRequestHandler<UpdateAuthorCommand, Unit>
{
    public async Task<Unit> Handle(UpdateAuthorCommand request, CancellationToken cancellationToken)
    {
        var authorId =  request.AuthorId;
        
        var authorEntity = await unitOfWork.AuthorRepository.GetTrackedAuthorByIdAsync(authorId, cancellationToken);
        if (authorEntity == null)
        {
            throw new NotFoundException($"Author with id {authorId} not found");
        }

        AuthorMapper.CommandToEntityInUpdate(request, ref authorEntity);
        
        await unitOfWork.AuthorRepository.Update(authorEntity, cancellationToken);
        
        return Unit.Value;
    }
}