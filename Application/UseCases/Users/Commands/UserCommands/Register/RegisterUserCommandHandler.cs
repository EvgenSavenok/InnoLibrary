using Application.MappingProfiles.Users;
using Domain.Entities.User;
using Domain.ErrorHandlers;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.UseCases.Users.Commands.UserCommands.Register;

public class RegisterUserCommandHandler(UserManager<User> userManager)
    : IRequestHandler<RegisterUserCommand, IdentityResult>
{
    public async Task<IdentityResult> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var userDto = request.UserForRegistrationDto;
        
        var user = UserMapper.DtoToEntity(userDto);
        
        var existingUser = await userManager.FindByNameAsync(userDto.UserName);
        if (existingUser != null)
        {
            throw new AlreadyExistsException("Users with such username already exists.");
        }
        
        var existingEmail = await userManager.FindByEmailAsync(userDto.Email);
        if (existingEmail != null)
        {
            throw new AlreadyExistsException("Users with such email already exists.");
        }
        
        var result = await userManager.CreateAsync(user, userDto.Password);
        if (result.Succeeded)
        {
            var userRoleAsString = userDto.Role.ToString();
            await userManager.AddToRolesAsync(user, new List<string> { userRoleAsString });
        }
        else
        {
            throw new BadRequestException($"Cannot create a new user, because {result}");
        }
        
        return result;
    }
}
