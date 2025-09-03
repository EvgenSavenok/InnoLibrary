using Application.RequestFeatures;
using Domain.Entities.Booking;

namespace Application.Contracts.Repository.Booking;

public interface IAuthorRepository : IBaseRepository<Author>
{
    public Task<Author?> GetAuthorByIdAsync(
        int authorId,
        CancellationToken cancellationToken);

    public Task<Author?> GetTrackedAuthorByIdAsync(
        int authorId,
        CancellationToken cancellationToken);

    public Task<PagedResult<Author>> GetAllAuthorsAsync(
        AuthorQueryParameters parameters,
        CancellationToken cancellationToken);
}