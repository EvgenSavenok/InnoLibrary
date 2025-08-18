using Application.Contracts.RepositoryContracts.Booking;
using Application.MappingProfiles.Booking;
using Domain.Entities.Booking;
using MediatR;

namespace Application.UseCases.Booking.Commands.AuthorCommands.CreateAuthor;

public class CreateAuthorCommandHandler (
    IUnitOfWork unitOfWork)
    : IRequestHandler<CreateAuthorCommand, Author>
{
    public async Task<Author> Handle(CreateAuthorCommand request, CancellationToken cancellationToken)
    {
        var authorEntity = AuthorMapper.CommandToEntity(request);
        
        await unitOfWork.AuthorRepository.Create(authorEntity,  cancellationToken);
        
        return authorEntity;
    }
}