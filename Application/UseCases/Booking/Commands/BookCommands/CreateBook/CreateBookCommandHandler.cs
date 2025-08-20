using Application.Contracts.RepositoryContracts.Booking;
using Application.DTO.Booking.BookDto;
using Application.MappingProfiles.Booking;
using Domain.Entities.Booking;
using FluentValidation;
using MediatR;

namespace Application.UseCases.Booking.Commands.BookCommands.CreateBook;

public class CreateBookCommandHandler(
    IUnitOfWork unitOfWork,
    IValidator<Book> bookValidator)
    : IRequestHandler<CreateBookCommand, CreateBookResponseDto>
{
    public async Task<CreateBookResponseDto> Handle(CreateBookCommand request, CancellationToken cancellationToken)
    {
        var bookEntity = BookMapper.CommandToEntity(request);
        
        var existingAuthors = (await unitOfWork.AuthorRepository
                .FindByConditionTrackedAsync(
                    author => request.BookDto.AuthorIds.Contains(author.AuthorId), 
                    cancellationToken))
            .ToList();
        bookEntity.BookAuthors = existingAuthors;
        
        var validationResult = await bookValidator.ValidateAsync(bookEntity, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        await unitOfWork.BookRepository.CreateAsync(bookEntity, cancellationToken);

        return new CreateBookResponseDto { BookId = bookEntity.Id };
    }
}