using Application.Contracts.Repository.Booking;
using Application.DTO.Booking.AuthorDto;
using Application.MappingProfiles.Booking;
using Domain.ErrorHandlers;
using MediatR;

namespace Application.UseCases.Booking.Queries.AuthorQueries.GetAuthorByBookId;

public class GetAuthorByIdQueryHandler(
    IUnitOfWork unitOfWork) : IRequestHandler<GetAuthorByIdQuery, AuthorDto>
{
    public async Task<AuthorDto> Handle(GetAuthorByIdQuery request, CancellationToken cancellationToken)
    {
        int authorId = request.AuthorId;
        
        var authorEntity = await unitOfWork.AuthorRepository.GetAuthorByIdAsync(authorId, cancellationToken);
        if (authorEntity == null)
        {
            throw new NotFoundException($"Author with id {authorId} not found.");
        }

        var authorDto = AuthorMapper.EntityToDto(authorEntity);
        
        return authorDto;
    }
}