using Application.Contracts.RepositoryContracts.Booking;
using Application.DTO.Booking.BookDto;
using Application.MappingProfiles.Booking;
using Application.RequestFeatures;
using MediatR;

namespace Application.UseCases.Booking.Queries.BookQueries.GetAllBooks;

public class GetAllBooksQueryHandler(
    IUnitOfWork unitOfWork)
    : IRequestHandler<GetAllBooksQuery, PagedResult<BookDto>>
{
    public async Task<PagedResult<BookDto>> Handle(GetAllBooksQuery request, CancellationToken cancellationToken)
    {
        var booksPaged = await unitOfWork.BookRepository.GetAllBooksAsync(request.Parameters, cancellationToken);

        return new PagedResult<BookDto>
        {
            Items = BookMapper.EntitiesToDtos(booksPaged.Items),
            TotalCount = booksPaged.TotalCount,
            PageNumber = booksPaged.PageNumber,
            PageSize = booksPaged.PageSize
        };
    }
}