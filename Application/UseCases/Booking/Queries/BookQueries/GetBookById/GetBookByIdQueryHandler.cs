using Application.Contracts.Repository.Booking;
using Application.DTO.Booking.BookDto;
using Application.MappingProfiles.Booking;
using Domain.ErrorHandlers;
using MediatR;

namespace Application.UseCases.Booking.Queries.BookQueries.GetBookById;

public class GetBookByIdQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetBookByIdQuery, BookDto>
{
    public async Task<BookDto> Handle(GetBookByIdQuery request, CancellationToken cancellationToken)
    {
        int bookId = request.BookId;
        
        var bookEntity = await unitOfWork.BookRepository.GetBookByIdAsync(bookId, cancellationToken);

        if (bookEntity == null)
        {
            throw new NotFoundException($"Book with id {bookId} not found.");
        }

        var bookDto = BookMapper.EntityToDto(bookEntity);
        
        return bookDto;
    }
}