using Application.DTO.Users.TokenDto;
using Application.DTO.Users.UserDto;
using Domain.Entities.User;

namespace Application.Contracts.Users;

public interface IAuthManagerService
{
    Task<bool> ValidateUser(UserForAuthenticationDto userForAuth);
    
    Task<TokenDto> CreateTokens(User user, bool populateExp);
    
    public Task<string> CreateAccessToken(User user);
}