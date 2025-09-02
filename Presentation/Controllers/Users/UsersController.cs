using Application.DTO.Users.UserDto;
using Application.UseCases.Users.Commands.UserCommands.Authenticate;
using Application.UseCases.Users.Commands.UserCommands.Register;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers.Users;

[Route("api/users")]
[ApiController]
public class UsersController(IMediator mediator) : Controller
{
    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser(
        [FromBody] UserForRegistrationDto userForRegistration)
    {
        var command = new RegisterUserCommand
        {
            UserForRegistrationDto = userForRegistration
        };
        await mediator.Send(command);
        
        return Ok();
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Authenticate([FromBody] UserForAuthenticationDto userForLogin)
    {
        var command = new AuthenticateUserCommand
        {
            UserForAuthenticationDto = userForLogin,
            HttpContext = HttpContext
        };
        var accessToken = await mediator.Send(command);
         
        return Ok(new { AccessToken = accessToken });
    }
}
