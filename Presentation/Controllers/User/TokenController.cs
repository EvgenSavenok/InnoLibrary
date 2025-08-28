using Application.DTO.User.TokenDto;
using Application.UseCases.User.Commands.TokenCommands.RefreshToken;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers.User;

[Route("api/token")]
[ApiController]
public class TokenController(
    IMediator mediator) : Controller
{
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] UpdateAccessTokenDto dto)
    {
        if (string.IsNullOrEmpty(dto.AccessToken))
        {
            return BadRequest("AccessToken is required.");
        }
        
        var command = new RefreshTokenCommand
        {
            AccessToken = dto.AccessToken,
            HttpContext = HttpContext
        };
        var refreshedAccessToken = await mediator.Send(command);
        
        return Ok(new UpdateAccessTokenDto { AccessToken = refreshedAccessToken });
    }
}
