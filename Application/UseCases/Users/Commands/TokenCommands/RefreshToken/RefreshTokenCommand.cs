using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.UseCases.Users.Commands.TokenCommands.RefreshToken;

public record RefreshTokenCommand : IRequest<string>
{
    public string AccessToken { get; set; }
    
    public HttpContext HttpContext { get; set; }
}