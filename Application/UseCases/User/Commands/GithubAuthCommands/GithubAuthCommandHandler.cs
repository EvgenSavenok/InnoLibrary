using System.Security.Claims;
using Application.Contracts.User;
using Application.DTO.Users.TokenDto;
using Domain.ErrorHandlers;
using Duende.IdentityServer.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.UseCases.User.Commands.GithubAuthCommands;

public class GithubAuthCommandHandler(
    UserManager<Domain.Entities.User.User> userManager,
    IAuthManagerService authManager) : IRequestHandler<GithubAuthCommand, TokenDto>
{
    private const string Provider = "GitHub";
    
    public async Task<TokenDto> Handle(GithubAuthCommand request, CancellationToken cancellationToken)
    {
        var authResult = request.AuthResult;    
        if (!authResult.Succeeded)
        {
            throw new BadRequestException("External authentication failed.");
        }

        var principal = authResult.Principal;

        var emailClaim = principal.FindFirst(ClaimTypes.Email)?.Value;
        if (string.IsNullOrEmpty(emailClaim))
        {
            throw new BadRequestException("Email is required.");
        }

        var user = await userManager.FindByEmailAsync(emailClaim);
        if (user is null)
        {
            user = await CreateNewUserAsync(principal);
        }

        await EnsureLoginExistsAsync(user, principal, cancellationToken);

        authManager.
        // var accessToken = await _tokenService.GenerateAccessToken(user);
        // var refreshToken = _tokenService.GenerateRefreshToken();
        // var tokenHash = _tokenHasher.HashToken(refreshToken);

        // var refreshTokenEntity = new RefreshToken
        // {
        //     Id = Guid.NewGuid().ToString(),
        //     TokenHash = tokenHash,
        //     CreatedAt = DateTime.UtcNow,
        //     ExpiresAt = DateTime.UtcNow.AddDays(_jwtSettings.Value.RefreshTokenExpirationInDays),
        //     UserId = user.Id
        // };
        //
        // await _refreshTokensRepository.AddAsync(refreshTokenEntity, cancellationToken);

        return new TokenDto(accessToken, refreshToken);
    }
    
    private async Task<Domain.Entities.User.User> CreateNewUserAsync(ClaimsPrincipal principal)
    {
        var user = new Domain.Entities.User.User
        {
            Id = Guid.NewGuid().ToString(),
            Email = principal.FindFirst(ClaimTypes.Email)?.Value,
            EmailConfirmed = true,
            UserName = principal.FindFirst(ClaimTypes.Name)?.Value ?? "",
            FirstName = principal.FindFirst(ClaimTypes.GivenName)?.Value ?? "",
            LastName = principal.FindFirst(ClaimTypes.Surname)?.Value ?? "",
        };

        var result = await userManager.CreateAsync(user);
        if (!result.Succeeded)
        {
            throw new BadRequestException("Failed to create new user.");
        }

        return user;
    }
    
    private async Task EnsureLoginExistsAsync(
        Domain.Entities.User.User user, 
        ClaimsPrincipal principal,
        CancellationToken cancellationToken)
    {
        var logins = await userManager.GetLoginsAsync(user);

        var existingLogin = logins.FirstOrDefault(l => l.ProviderKey == Provider);
        if (existingLogin is not null)
        {
            return;
        }

        var providerKey = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(providerKey))
        {
            return;
        }

        var userLoginInfo = new UserLoginInfo(Provider, providerKey, Provider);
        var result = await userManager.AddLoginAsync(user, userLoginInfo);
    }
}