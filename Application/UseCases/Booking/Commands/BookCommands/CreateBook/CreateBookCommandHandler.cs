using Application.Contracts.RepositoryContracts.Booking;
using Application.DTO.Booking.BookDto;
using Application.MappingProfiles.Booking;
using MediatR;

namespace Application.UseCases.Booking.Commands.BookCommands.CreateBook;

public class CreateBookCommandHandler(
    IUnitOfWork unitOfWork)
    : IRequestHandler<CreateBookCommand, CreateBookResponseDto>
{
    public async Task<CreateBookResponseDto> Handle(CreateBookCommand request, CancellationToken cancellationToken)
    {
        var bookEntity = BookMapper.CommandToEntity(request);
        
        var existingAuthors = (await unitOfWork.AuthorRepository
                .FindByConditionTracked(
                    author => request.BookDto.AuthorIds.Contains(author.AuthorId), 
                    cancellationToken))
            .ToList();
        bookEntity.BookAuthors = existingAuthors;

        await unitOfWork.BookRepository.CreateAsync(bookEntity, cancellationToken);

        return new CreateBookResponseDto { BookId = bookEntity.Id };
    }
}