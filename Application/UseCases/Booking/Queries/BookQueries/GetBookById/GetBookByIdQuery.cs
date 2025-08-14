using Application.DTO.Booking.BookDto;
using MediatR;

namespace Application.UseCases.Booking.Queries.BookQueries.GetBookById;

public record GetBookByIdQuery(int BookId) : IRequest<BookDto>;