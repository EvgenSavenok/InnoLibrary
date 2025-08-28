namespace Application.DTO.User.UserDto;

public class UserForRegistrationDto
{
    public enum UserRole
    {
        User = 1,
        Administrator = 2
    }
    
    public string FirstName { get; set; }
    
    public string LastName { get; set; }
    
    public string UserName { get; set; }
    
    public string Password { get; set; }
    
    public string Email { get; set; }
    
    public UserRole Role { get; set; }
}