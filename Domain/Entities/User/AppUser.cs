using Microsoft.AspNetCore.Identity;

namespace Domain.Entities.User;

public class AppUser : IdentityUser
{
    public string? FirstName { get; set; }
    
    public string? LastName { get; set; }
    
    public string? RefreshToken { get; set; }
    
    public DateTime? RefreshTokenExpireTime { get; set; }
}