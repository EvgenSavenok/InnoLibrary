using Domain.Entities.User;
using UserDto = Application.DTO.User.UserDto.UserForRegistrationDto;

namespace Application.MappingProfiles.User;

public static class UserMapper 
{
   public static AppUser DtoToEntity(UserDto userDto)
   {
      return new AppUser
      {
         FirstName = userDto.FirstName,
         LastName = userDto.LastName,
         Email = userDto.Email,
         UserName = userDto.UserName
      };
   }
}
