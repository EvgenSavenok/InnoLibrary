using Application.DTO.Users.UserDto;
using Domain.Entities.User;

namespace Application.MappingProfiles.Users;

public static class UserMapper 
{
   public static User DtoToEntity(UserForRegistrationDto userDto)
   {
      return new User
      {
         FirstName = userDto.FirstName,
         LastName = userDto.LastName,
         Email = userDto.Email,
         UserName = userDto.UserName
      };
   }
}
