using Application.DTO.Booking.BookDto;
using Application.RequestFeatures;
using Application.UseCases.Booking.Commands.BookCommands.CreateBook;
using Application.UseCases.Booking.Commands.BookCommands.DeleteBook;
using Application.UseCases.Booking.Commands.BookCommands.UpdateBook;
using Application.UseCases.Booking.Queries.BookQueries.GetAllBooks;
using Application.UseCases.Booking.Queries.BookQueries.GetBookById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers.Booking;

[ApiController]
[Route("api/books")]
public class BookController(IMediator mediator): Controller
{
    [HttpGet("getBookById/{bookId}")]
    public async Task<IActionResult> GetBookById(int bookId, CancellationToken cancellationToken)
    {
        var query = new GetBookByIdQuery(bookId) { BookId = bookId };
        var book =  await mediator.Send(query, cancellationToken);
        
        return Ok(book);
    }

    [HttpGet("getAllBooks")]
    public async Task<IActionResult> GetAllBooks(
        [FromQuery] BookQueryParameters parameters, 
        CancellationToken cancellationToken)
    {
        var query = new GetAllBooksQuery { Parameters = parameters};
        var books = await mediator.Send(query, cancellationToken);
        
        return Ok(books);
    }
    
    [HttpPost("addBook")]
    public async Task<IActionResult> CreateBook([FromBody] BookDto bookDto)
    {
        var command = new CreateBookCommand
        {
            BookDto = bookDto
        };
        var createBookResponse = await mediator.Send(command);
        
        return Ok(createBookResponse.BookId.ToString());
    }
    
    [HttpPut("updateBook")]
    public async Task<IActionResult> UpdateBook(
        [FromBody] BookDto bookDto, 
        CancellationToken cancellationToken)
    {
        var command = new UpdateBookCommand
        {
            BookDto = bookDto
        };
        await mediator.Send(command, cancellationToken);
        
        return NoContent();
    }

    [HttpDelete("deleteBook/{bookId}")]
    public async Task<IActionResult> DeleteBook(int bookId, CancellationToken cancellationToken)
    {
        var command = new DeleteBookCommand { BookId = bookId };
        await mediator.Send(command, cancellationToken);
        
        return NoContent();
    }
}