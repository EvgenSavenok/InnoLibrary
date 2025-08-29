using Application.DTO.Users.TokenDto;
using MediatR;
using Microsoft.AspNetCore.Authentication;

namespace Application.UseCases.User.Commands.GithubAuthCommands;

public record GithubAuthCommand(AuthenticateResult AuthResult) : IRequest<TokenDto>;