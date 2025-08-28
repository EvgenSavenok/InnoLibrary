using Application.DTO.User;
using Application.DTO.User.UserDto;

namespace Application.MappingProfiles.User;
using Domain.Entities.User;

public static class UserMapper 
{
   public static AppUser DtoToEntity(UserForRegistrationDto userForRegistrationDto)
   {
      return new AppUser
      {
         FirstName = userForRegistrationDto.FirstName,
         LastName = userForRegistrationDto.LastName,
         Email = userForRegistrationDto.Email,
         UserName = userForRegistrationDto.UserName
      };
   }
}
