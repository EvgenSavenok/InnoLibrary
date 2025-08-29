using Application.DTO.Users.UserDto;
using Domain.Entities.User;

namespace Application.MappingProfiles.Users;

public static class UserMapper 
{
   public static User DtoToEntity(UserForRegistrationDto userForRegistrationDto)
   {
      return new User
      {
         FirstName = userForRegistrationDto.FirstName,
         LastName = userForRegistrationDto.LastName,
         Email = userForRegistrationDto.Email,
         UserName = userForRegistrationDto.UserName,
      };
   }
}
