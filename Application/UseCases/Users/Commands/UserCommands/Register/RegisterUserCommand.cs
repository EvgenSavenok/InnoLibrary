using Application.DTO.Users.UserDto;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.UseCases.Users.Commands.UserCommands.Register;

public record RegisterUserCommand : IRequest<IdentityResult>
{
    public UserForRegistrationDto UserForRegistrationDto { get; set; }
}