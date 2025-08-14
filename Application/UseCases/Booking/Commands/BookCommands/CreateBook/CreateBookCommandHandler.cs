using Application.Contracts.RepositoryContracts.Booking;
using Application.DTO.Booking.BookDto;
using Application.MappingProfiles.Booking;
using MediatR;

namespace Application.UseCases.Booking.Commands.BookCommands.CreateBook;

public class CreateBookCommandHandler(
    IUnitOfWork unitOfWork)
    : IRequestHandler<CreateBookCommand, CreateBookRepsponseDto>
{
    public async Task<CreateBookRepsponseDto> Handle(CreateBookCommand request, CancellationToken cancellationToken)
    {
        var bookEntity = BookMapper.CommandToEntity(request);

        await unitOfWork.BookRepository.Create(bookEntity, cancellationToken);

        return new CreateBookRepsponseDto { BookId = bookEntity.Id };
    }
}