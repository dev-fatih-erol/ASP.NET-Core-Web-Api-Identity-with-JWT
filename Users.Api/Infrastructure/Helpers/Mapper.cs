using Users.Api.Data.Entities;
using Users.Api.Models;

namespace Users.Api.Infrastructure.Helpers
{
    public class Mapper
    {
        public UserDto MapToUserDto(User user)
        {
            var userDto = new UserDto()
            {
                Id = user.Id,
                Name = user.Name,
                Surname = user.Surname,
                UserName = user.UserName
            };

            return userDto;
        }

        public MeDto MapToMeDto(User user)
        {
            var meDto = new MeDto()
            {
                Id = user.Id,
                Name = user.Name,
                Surname = user.Surname,
                UserName = user.UserName,
                PhoneNumber = user.PhoneNumber
            };

            return meDto;
        }
    }
}