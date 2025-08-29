using Domain.Entities.User;
using Duende.IdentityServer;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers.Users;

[ApiController]
[Route("api/user/github")]
public class GitHubAuthController(
    IMediator mediator,
    SignInManager<User> signInManager)
    : Controller
{
    private const string Provider = "GitHub";

    [HttpPost("login")]
    public IActionResult TriggerExternalLogin()
    {
        var redirectUrl = Url.Action(nameof(HandleExternalLoginCallback), "GitHubAuth");
        var properties = signInManager.ConfigureExternalAuthenticationProperties(Provider, redirectUrl);

        return Challenge(properties, Provider);
    }

    [HttpGet("callback")]
    public async Task<IActionResult> HandleExternalLoginCallback(CancellationToken cancellationToken = default)
    {
        var authenticateResult = await HttpContext.AuthenticateAsync(IdentityServerConstants.ExternalCookieAuthenticationScheme);
        var tokensDto = await mediator.Send(authenticateResult, cancellationToken);

        await HttpContext.SignOutAsync(IdentityServerConstants.ExternalCookieAuthenticationScheme);

        return Ok(tokensDto);
    }
}