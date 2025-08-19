using Application.DTO.Booking.AuthorDto;
using Application.RequestFeatures;
using Application.UseCases.Booking.Commands.AuthorCommands.CreateAuthor;
using Application.UseCases.Booking.Commands.AuthorCommands.DeleteAuthor;
using Application.UseCases.Booking.Commands.AuthorCommands.UpdateAuthor;
using Application.UseCases.Booking.Queries.AuthorQueries.GetAllAuthors;
using Application.UseCases.Booking.Queries.AuthorQueries.GetAuthorByBookId;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers.Booking;

[ApiController]
[Route("api/authors")]
public class AuthorController(IMediator mediator) : Controller
{
    [HttpGet("getAuthorById/{authorId}")]
    public async Task<IActionResult> GetBookByIdAsync(int authorId, CancellationToken cancellationToken)
    {
        var query = new GetAuthorByIdQuery(authorId) { AuthorId = authorId };
        var author =  await mediator.Send(query, cancellationToken);
        
        return Ok(author);
    }
    
    [HttpGet("getAllAuthors")]
    public async Task<IActionResult> GetAllAuthorsAsync(
        [FromQuery] AuthorQueryParameters parameters, 
        CancellationToken cancellationToken)
    {
        var query = new GetAllAuthorsQuery { Parameters = parameters};
        var authors = await mediator.Send(query, cancellationToken);
        
        return Ok(authors);
    }
    
    [HttpPost("addAuthor")]
    public async Task<IActionResult> AddAuthorAsync(
        [FromBody]AuthorDto authorDto, 
        CancellationToken cancellationToken)
    {
        var command = new CreateAuthorCommand
        {
            AuthorDto = authorDto
        };
        var createdAuthorDto = await mediator.Send(command, cancellationToken);
        
        return Ok(createdAuthorDto.AuthorId.ToString());
    }

    [HttpPut("updateAuthor/{authorId}")]
    public async Task<IActionResult> UpdateAuthorAsync(
        [FromBody] AuthorDto authorDto,
        int authorId,
        CancellationToken cancellationToken)
    {
        var command = new UpdateAuthorCommand
        {
            AuthorId = authorId,
            AuthorDto = authorDto
        };
        
        await mediator.Send(command, cancellationToken);

        return Ok();
    }

    [HttpDelete("deleteAuthor/{authorId}")]
    public async Task<IActionResult> DeleteAuthorAsync(
        int authorId,
        CancellationToken cancellationToken)
    {
        var command = new DeleteAuthorCommand
        {
            AuthorId = authorId
        };
        await mediator.Send(command, cancellationToken);
    
        return Ok();
    }
}