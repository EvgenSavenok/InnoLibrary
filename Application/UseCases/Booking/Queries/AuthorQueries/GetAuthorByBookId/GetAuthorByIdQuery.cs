using Application.DTO.Booking.AuthorDto;
using MediatR;

namespace Application.UseCases.Booking.Queries.AuthorQueries.GetAuthorByBookId;

public record GetAuthorByIdQuery(int AuthorId) : IRequest<AuthorDto>;