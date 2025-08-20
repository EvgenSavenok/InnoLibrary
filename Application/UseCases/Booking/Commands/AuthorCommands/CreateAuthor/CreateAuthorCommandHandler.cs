using Application.Contracts.RepositoryContracts.Booking;
using Application.DTO.Booking.AuthorDto;
using Application.MappingProfiles.Booking;
using Domain.Entities.Booking;
using MediatR;

namespace Application.UseCases.Booking.Commands.AuthorCommands.CreateAuthor;

public class CreateAuthorCommandHandler (
    IUnitOfWork unitOfWork)
    : IRequestHandler<CreateAuthorCommand, Unit>
{
    public async Task<Unit> Handle(CreateAuthorCommand request, CancellationToken cancellationToken)
    {
        var authorEntity = AuthorMapper.CommandToEntity(request);
        
        await unitOfWork.AuthorRepository.CreateAsync(authorEntity,  cancellationToken);
        
        return Unit.Value;
    }
}