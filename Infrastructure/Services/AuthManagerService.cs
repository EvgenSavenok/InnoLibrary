using Application.Contracts.User;
using Domain.Entities.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services;

public class AuthManagerService(
    UserManager<User> userManager, 
    IConfiguration configuration) : IAuthManagerService
{
    
}