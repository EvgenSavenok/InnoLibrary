using Application.Contracts.RepositoryContracts.Booking;
using Application.MappingProfiles.Booking;
using Domain.ErrorHandlers;
using MediatR;

namespace Application.UseCases.Booking.Commands.BookCommands.UpdateBook;

public class UpdateBookCommandHandler(
    IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateBookCommand, Unit>
{
    public async Task<Unit> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
    {
        var bookEntity = await unitOfWork.BookRepository.GetTrackedBookByIdAsync(
            request.BookDto.Id,
            cancellationToken);

        if (bookEntity == null)
        {
            throw new NotFoundException($"Book with id {request.BookDto.Id} not found.");
        }
        
        BookMapper.CommandToEntityInUpdate(request, ref bookEntity);

        var authorsToAdd = (await unitOfWork.AuthorRepository.FindByConditionTracked(
             author => request.BookDto.AuthorIds.Contains(author.AuthorId),
             cancellationToken)).ToList();
        bookEntity.BookAuthors = authorsToAdd;

        await unitOfWork.BookRepository.Update(bookEntity, cancellationToken);

        return Unit.Value;
    }
}