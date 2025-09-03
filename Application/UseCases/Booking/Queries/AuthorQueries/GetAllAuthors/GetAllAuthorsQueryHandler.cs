using Application.Contracts.Repository.Booking;
using Application.DTO.Booking.AuthorDto;
using Application.MappingProfiles.Booking;
using Application.RequestFeatures;
using MediatR;

namespace Application.UseCases.Booking.Queries.AuthorQueries.GetAllAuthors;

public class GetAllAuthorsQueryHandler(
    IUnitOfWork unitOfWork) : IRequestHandler<GetAllAuthorsQuery, PagedResult<AuthorDto>>
{
    public async Task<PagedResult<AuthorDto>> Handle(GetAllAuthorsQuery request, CancellationToken cancellationToken)
    {
        var booksPaged = await unitOfWork.AuthorRepository.GetAllAuthorsAsync(request.Parameters, cancellationToken);

        return new PagedResult<AuthorDto>
        {
            Items = AuthorMapper.EntitiesToDtos(booksPaged.Items),
            TotalCount = booksPaged.TotalCount,
            PageNumber = booksPaged.PageNumber,
            PageSize = booksPaged.PageSize
        };
    }
}