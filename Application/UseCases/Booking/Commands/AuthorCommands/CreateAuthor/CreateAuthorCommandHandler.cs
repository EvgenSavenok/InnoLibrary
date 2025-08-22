using Application.Contracts.RepositoryContracts.Booking;
using Application.MappingProfiles.Booking;
using Domain.Entities.Booking;
using FluentValidation;
using MediatR;

namespace Application.UseCases.Booking.Commands.AuthorCommands.CreateAuthor;

public class CreateAuthorCommandHandler (
    IUnitOfWork unitOfWork,
    IValidator<Author> validator)
    : IRequestHandler<CreateAuthorCommand, Unit>
{
    public async Task<Unit> Handle(CreateAuthorCommand request, CancellationToken cancellationToken)
    {
        var authorEntity = AuthorMapper.CommandToEntity(request);

        var validationResult = await validator.ValidateAsync(authorEntity, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }
        
        await unitOfWork.AuthorRepository.CreateAsync(authorEntity,  cancellationToken);
        
        return Unit.Value;
    }
}