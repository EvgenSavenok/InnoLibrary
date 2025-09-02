using System.Security.Claims;
using Duende.IdentityServer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions;

public static class OAuthExtension
{
    public static void AddAuthConfiguration(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
        })
        .AddCookie(IdentityServerConstants.ExternalCookieAuthenticationScheme)
        .AddGitHub(options =>
        {
            options.CallbackPath = new PathString(configuration["GitHubAuthSettings:CallbackUri"]!);
            options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
            options.ClientId = configuration["GitHubAuthSettings:ClientId"]!;
            options.ClientSecret = configuration["GitHubAuthSettings:ClientSecret"]!;

            options.SaveTokens = true;

            options.Scope.Add("user:email");

            options.ClaimActions.MapJsonKey(ClaimTypes.Name, "login");
            options.ClaimActions.MapJsonKey(ClaimTypes.Email, "email");
            options.ClaimActions.MapJsonKey("urn:github:avatar", "avatar_url");
        })
        .AddGoogle(options =>
        {
            options.CallbackPath = new PathString(configuration["GoogleAuthSettings:CallbackUri"]!);
            options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
            options.ClientId = configuration["GoogleAuthSettings:ClientId"]!;
            options.ClientSecret = configuration["GoogleAuthSettings:ClientSecret"]!;
            
            options.SaveTokens = true;
            
            options.ClaimActions.MapJsonKey(ClaimTypes.Name, "name");
            options.ClaimActions.MapJsonKey(ClaimTypes.Email, "email");
            options.ClaimActions.MapJsonKey("urn:google:picture", "picture");
        });
    }
}