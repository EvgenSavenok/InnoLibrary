using Application.Contracts.User;
using Domain.ErrorHandlers;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.UseCases.User.Commands.UserCommands.Authenticate;

public class AuthenticateUserCommandHandler(
    UserManager<Domain.Entities.User.User> userManager,
    IAuthenticationManager authManager)
    : IRequestHandler<AuthenticateUserCommand, Unit>
{
    public async Task<Unit> Handle(
        AuthenticateUserCommand request,
        CancellationToken cancellationToken)
    {
        var userForLogin = request.UserForAuthenticationDto;
        
        var user = await userManager.FindByNameAsync(userForLogin.UserName);
        if (user == null || !await userManager.CheckPasswordAsync(user, userForLogin.Password))
        {
            throw new UnauthorizedException("Cannot find user");
        }
        
        await authManager.ValidateUser(userForLogin);
        
        return Unit.Value;
    }
}