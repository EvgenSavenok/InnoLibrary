using System.Security.Claims;
using Application.Contracts.Users;
using Domain.Entities.User;
using Domain.ErrorHandlers;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Application.UseCases.Users.Commands.UserCommands.GoogleAuth;

public class GoogleAuthCommandHandler(
    UserManager<User> userManager,
    IAuthManagerService authManager) : IRequestHandler<GoogleAuthCommand, string>
{
    private const string Provider = "Google";
    
    public async Task<string> Handle(GoogleAuthCommand request, CancellationToken cancellationToken)
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

        await EnsureLoginExistsAsync(user, principal);

        var tokens = await authManager.CreateTokens(user, populateExp: true);
        if (tokens.AccessToken == null || tokens.RefreshToken == null)
        {
            throw new UnauthorizedException("Cannot create access or refresh token");
        }
        
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true, 
            SameSite = SameSiteMode.None,
            Expires = DateTime.UtcNow.AddDays(7)
        };

        request.HttpContext.Response.Cookies.Append("refreshToken", tokens.RefreshToken, cookieOptions);
        
        return tokens.AccessToken;
    }
    
    private async Task<User> CreateNewUserAsync(ClaimsPrincipal principal)
    {
        var user = new User
        {
            Id = Guid.NewGuid().ToString(),
            Email = principal.FindFirst(ClaimTypes.Email)?.Value,
            EmailConfirmed = true,
            FirstName = principal.FindFirst(ClaimTypes.GivenName)?.Value ?? "",
            LastName = principal.FindFirst(ClaimTypes.Surname)?.Value ?? "",
        };
        user.UserName = user.Email;

        var result = await userManager.CreateAsync(user);
        if (!result.Succeeded)
        {
            throw new BadRequestException("Failed to create new user.");
        }
        
        var userRoleAsString = "User";
        await userManager.AddToRolesAsync(user, new List<string> { userRoleAsString });

        return user;
    }
    
    private async Task EnsureLoginExistsAsync(
        User user, 
        ClaimsPrincipal principal)
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
        await userManager.AddLoginAsync(user, userLoginInfo);
    }
}