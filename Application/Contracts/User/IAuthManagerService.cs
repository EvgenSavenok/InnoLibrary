using Application.DTO.User.TokenDto;
using Application.DTO.User.UserDto;
using Domain.Entities.User;

namespace Application.Contracts.User;

public interface IAuthManagerService
{
    Task<bool> ValidateUser(UserForAuthenticationDto userForAuth);
    
    Task<TokenDto> CreateTokens(AppUser user, bool populateExp);
    
    public Task<string> CreateAccessToken(AppUser user);
}