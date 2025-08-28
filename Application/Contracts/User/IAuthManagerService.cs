using Application.DTO.User.TokenDto;
using Application.DTO.User.UserDto;

namespace Application.Contracts.User;

public interface IAuthManagerService
{
    Task<bool> ValidateUser(UserForAuthenticationDto userForAuth);
    
    Task<TokenDto> CreateTokens(Domain.Entities.User.User user, bool populateExp);
    
    public Task<string> CreateAccessToken(Domain.Entities.User.User user);
}