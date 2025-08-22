using Application.DTO.User;

namespace Application.Contracts.User;

public interface IAuthenticationManager
{
    Task<bool> ValidateUser(UserForAuthenticationDto userForAuth);
}