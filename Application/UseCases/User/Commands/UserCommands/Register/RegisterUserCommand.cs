using Application.DTO.User;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.UseCases.User.Commands.UserCommands.Register;

public record RegisterUserCommand : IRequest<IdentityResult>
{
    public UserForRegistrationDto UserForRegistrationDto { get; set; }
}