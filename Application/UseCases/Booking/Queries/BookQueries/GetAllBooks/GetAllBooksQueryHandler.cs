using Application.Contracts.RepositoryContracts.Booking;
using Application.DTO.Booking.BookDto;
using Application.MappingProfiles.Booking;
using MediatR;

namespace Application.UseCases.Booking.Queries.BookQueries.GetAllBooks;

public class GetAllBooksQueryHandler(
    IUnitOfWork unitOfWork)
    : IRequestHandler<GetAllBooksQuery, IEnumerable<BookDto>>
{
    public async Task<IEnumerable<BookDto>> Handle(GetAllBooksQuery request, CancellationToken cancellationToken)
    {
        var books = await unitOfWork.BookRepository.GetAllBooksAsync(cancellationToken);

        var bookDtos = BookMapper.EntitiesToDtos(books);

        return bookDtos;
    }
}