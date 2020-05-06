using Microsoft.AspNetCore.Identity;

namespace Users.Api.Data.Entities
{
    public class User : IdentityUser<int>
    {
        public string Name { get; set; }

        public string Surname { get; set; }
    }
}