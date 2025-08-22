using Application.DTO.User;
using MediatR;

namespace Application.UseCases.User.Commands.UserCommands.Authenticate;

public record AuthenticateUserCommand : IRequest<Unit>
{
    public UserForAuthenticationDto UserForAuthenticationDto { get; set; }
}