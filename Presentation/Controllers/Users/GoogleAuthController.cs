using Application.UseCases.Users.Commands.UserCommands.GithubAuth;
using Application.UseCases.Users.Commands.UserCommands.GoogleAuth;
using Domain.Entities.User;
using Duende.IdentityServer;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers.Users;

[ApiController]
[Route("api/auth/google")]
public class GoogleAuthController(
    IMediator mediator,
    SignInManager<User> signInManager)
    : Controller
{
    [HttpGet("callback")]
    public async Task<IActionResult> HandleExternalLoginCallback(CancellationToken cancellationToken = default)
    {
        var authenticateResult = await HttpContext.AuthenticateAsync(IdentityServerConstants.ExternalCookieAuthenticationScheme);
        
        var command = new GoogleAuthCommand(authenticateResult, HttpContext);
        var tokensDto = await mediator.Send(command, cancellationToken);

        await HttpContext.SignOutAsync(IdentityServerConstants.ExternalCookieAuthenticationScheme);

        return Ok(tokensDto);
    }
    
    [HttpGet("login")]
    public IActionResult TriggerExternalLogin()
    {
        var redirectUrl = Url.Action(
            action: nameof(HandleExternalLoginCallback),
            controller: "GoogleAuth",          
            values: null,
            protocol: Request.Scheme);

        var props = signInManager.ConfigureExternalAuthenticationProperties("Google", redirectUrl);
        
        return Challenge(props, "Google");
    }
}