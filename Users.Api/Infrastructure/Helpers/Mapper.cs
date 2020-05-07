using Users.Api.Data.Entities;
using Users.Api.Models;

namespace Users.Api.Infrastructure.Helpers
{
    public class Mapper
    {
        public static UserDto MapToUserDto(User user)
        {
            var userDto = new UserDto()
            {
                Id = user.Id,
                Name = user.Name,
                UserName = user.UserName
            };

            return userDto;
        }
    }
}