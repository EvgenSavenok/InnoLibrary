using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Application.Contracts.User;
using Application.DTO.User.TokenDto;
using Application.DTO.User.UserDto;
using Domain.Entities.User;
using Domain.ErrorHandlers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Services;

public class AuthManagerService(
    UserManager<AppUser> userManager, 
    IConfiguration configuration) : IAuthManagerService
{
    private AppUser _appUser;

    public async Task<bool> ValidateUser(UserForAuthenticationDto userForAuth)
    {
        _appUser = await userManager.FindByNameAsync(userForAuth.UserName);
        if (_appUser is null)
            throw new NotFoundException("Cannot find user");
        return await userManager.CheckPasswordAsync(_appUser, userForAuth.Password);
    }

    public async Task<TokenDto> CreateTokens(AppUser appUser, bool populateExp)
    {
        var jwtSettings = configuration.GetSection("JwtSettings");
        var refreshTokenLifeTime = jwtSettings.GetSection("RefreshTokenLifeTime").Value;
        if (refreshTokenLifeTime is null)
        {
            throw new NotFoundException("RefreshTokenLifeTime is null");
        }

        int doubleRefreshTokenLifeTime = Int32.Parse(refreshTokenLifeTime);
        
        _appUser = appUser;
        
        var signingCredentials = GetSigningCredentials();
        var claims = await GetClaims();
        var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
        
        var refreshToken = GenerateRefreshToken();
        _appUser.RefreshToken = refreshToken;

        if (populateExp)
        {
            _appUser.RefreshTokenExpireTime = DateTime.UtcNow.AddDays(doubleRefreshTokenLifeTime);
        }

        await userManager.UpdateAsync(_appUser);
        var accessToken = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        
        return new TokenDto(accessToken, refreshToken);
    }
    
    private SigningCredentials GetSigningCredentials()
    {
        var jwtSettings = configuration.GetSection("JwtSettings");
        
        var secretKey = jwtSettings.GetSection("validIssuer").Value;
        
        var key = Encoding.UTF8.GetBytes(secretKey!);
        
        var secret = new SymmetricSecurityKey(key);
        
        return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
    }
    
    private async Task<List<Claim>> GetClaims()
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, _appUser.UserName!),
            new(ClaimTypes.NameIdentifier, _appUser.Id)
        };
        
        var roles = await userManager.GetRolesAsync(_appUser);
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }
        
        return claims;
    }
    
    private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
    {
        var jwtSettings = configuration.GetSection("JwtSettings");
        var tokenOptions = new JwtSecurityToken
        (
            issuer: jwtSettings.GetSection("validIssuer").Value,
            audience: jwtSettings.GetSection("validAudience").Value,
            claims: claims,
            expires:
                DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings.GetSection("AccessTokenLifetime").Value)),
            signingCredentials: signingCredentials
        );
        
        return tokenOptions;
    }
    
    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        
        return Convert.ToBase64String(randomNumber);
    }
    
    public async Task<string> CreateAccessToken(AppUser appUser)
    {
        _appUser = appUser;
        
        var signingCredentials = GetSigningCredentials();
        var claims = await GetClaims();
        var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
        
        await userManager.UpdateAsync(_appUser);
        var accessToken = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        
        return accessToken;
    }
}