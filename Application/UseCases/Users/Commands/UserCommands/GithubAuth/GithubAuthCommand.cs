using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace Application.UseCases.Users.Commands.UserCommands.GithubAuth;

public record GithubAuthCommand(AuthenticateResult AuthResult, HttpContext HttpContext) : IRequest<string>;
