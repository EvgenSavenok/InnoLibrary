using Application.DTO.Booking.BookDto;
using MediatR;

namespace Application.UseCases.Booking.Queries.BookQueries.GetAllBooks;

public record GetAllBooksQuery : IRequest<IEnumerable<BookDto>>;