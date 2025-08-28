using Application.DTO.Users.UserDto;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.UseCases.Users.Commands.UserCommands.Authenticate;

public record AuthenticateUserCommand : IRequest<string>
{
    public UserForAuthenticationDto UserForAuthenticationDto { get; set; }
    
    public HttpContext HttpContext { get; set; } = null!;
}