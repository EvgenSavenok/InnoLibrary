using Application.Contracts.RepositoryContracts.Booking;
using Domain.ErrorHandlers;
using MediatR;

namespace Application.UseCases.Booking.Commands.BookCommands.DeleteBook;

public class DeleteBookCommandHandler(
    IUnitOfWork unitOfWork) 
    : IRequestHandler<DeleteBookCommand, Unit>
{
    public async Task<Unit> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
    {
        var bookId = request.BookId;
        
        var bookEntity = await unitOfWork.BookRepository.GetBookByIdAsync(
            bookId, 
            cancellationToken);
        if (bookEntity == null)
        {
            throw new NotFoundException($"Book with id {bookId} was not found");
        }

        await unitOfWork.BookRepository.Delete(bookEntity, cancellationToken);
        
        return Unit.Value;
    }
}