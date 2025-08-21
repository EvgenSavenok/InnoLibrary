using Application.Contracts.RepositoryContracts.Booking;
using Application.MappingProfiles.Booking;
using Domain.Entities.Booking;
using Domain.ErrorHandlers;
using FluentValidation;
using MediatR;

namespace Application.UseCases.Booking.Commands.AuthorCommands.UpdateAuthor;

public class UpdateAuthorCommandHandler(
    IUnitOfWork unitOfWork,
    IValidator<Author> validator) 
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
        
        var validationResult = await validator.ValidateAsync(authorEntity, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }
        
        await unitOfWork.AuthorRepository.UpdateAsync(authorEntity, cancellationToken);
        
        return Unit.Value;
    }
}