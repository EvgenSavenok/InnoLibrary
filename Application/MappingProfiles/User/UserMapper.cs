using Application.DTO.Users;
using Application.DTO.Users.UserDto;

namespace Application.MappingProfiles.User;
using Domain.Entities.User;

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
         PhoneNumber = userForRegistrationDto.PhoneNumber
      };
   }
}
