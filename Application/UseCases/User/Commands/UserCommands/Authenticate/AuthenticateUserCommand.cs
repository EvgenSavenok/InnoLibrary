using Application.DTO.User.UserDto;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.UseCases.User.Commands.UserCommands.Authenticate;

public record AuthenticateUserCommand : IRequest<string>
{
    public UserForAuthenticationDto UserForAuthenticationDto { get; set; }
    
    public HttpContext HttpContext { get; set; } = null!;
}