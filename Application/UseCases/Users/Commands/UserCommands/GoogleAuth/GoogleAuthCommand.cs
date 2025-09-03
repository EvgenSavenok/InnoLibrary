using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace Application.UseCases.Users.Commands.UserCommands.GoogleAuth;

public record GoogleAuthCommand(AuthenticateResult AuthResult, HttpContext HttpContext) : IRequest<string>;
