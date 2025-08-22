using Application.DTO.Booking.AuthorDto;
using Application.RequestFeatures;
using MediatR;

namespace Application.UseCases.Booking.Queries.AuthorQueries.GetAllAuthors;

public record GetAllAuthorsQuery : IRequest<PagedResult<AuthorDto>>
{
    public AuthorQueryParameters Parameters { get; set; }
}