using Application.DTO.Booking.BookDto;
using Application.RequestFeatures;
using Application.UseCases.Booking.Commands.BookCommands.CreateBook;
using Application.UseCases.Booking.Commands.BookCommands.DeleteBook;
using Application.UseCases.Booking.Commands.BookCommands.UpdateBook;
using Application.UseCases.Booking.Queries.BookQueries.GetAllBooks;
using Application.UseCases.Booking.Queries.BookQueries.GetBookById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers.Booking;

[ApiController]
[Route("api/books")]
public class BookController(IMediator mediator): Controller
{
    [HttpGet("getBookById/{bookId}")]
    [Authorize(Policy = "AdminOrUser")]
    public async Task<IActionResult> GetBookByIdAsync(int bookId, CancellationToken cancellationToken)
    {
        var query = new GetBookByIdQuery(bookId) { BookId = bookId };
        var book =  await mediator.Send(query, cancellationToken);
        
        return Ok(book);
    }

    [HttpGet("getAllBooks")]
    [Authorize(Policy = "AdminOrUser")]
    public async Task<IActionResult> GetAllBooksAsync(
        [FromQuery] BookQueryParameters parameters, 
        CancellationToken cancellationToken)
    {
        var query = new GetAllBooksQuery { Parameters = parameters};
        var books = await mediator.Send(query, cancellationToken);
        
        return Ok(books);
    }
    
    [HttpPost("addBook")]
    [Authorize(Policy = "Admin")]
    public async Task<IActionResult> CreateBookAsync([FromBody] BookDto bookDto)
    {
        var command = new CreateBookCommand
        {
            BookDto = bookDto
        };
        await mediator.Send(command);

        return NoContent();
    }
    
    [HttpPut("updateBook")]
    [Authorize(Policy = "Admin")]
    public async Task<IActionResult> UpdateBookAsync(
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
    [Authorize(Policy = "Admin")]
    public async Task<IActionResult> DeleteBookAsync(int bookId, CancellationToken cancellationToken)
    {
        var command = new DeleteBookCommand { BookId = bookId };
        await mediator.Send(command, cancellationToken);
        
        return NoContent();
    }
}