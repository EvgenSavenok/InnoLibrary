using Application.DTO.Booking.BookDto;
using Application.RequestFeatures;
using MediatR;

namespace Application.UseCases.Booking.Queries.BookQueries.GetAllBooks;

public record GetAllBooksQuery : IRequest<PagedResult<BookDto>>
{
    public BookQueryParameters Parameters { get; set; } = new();
}