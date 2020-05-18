using System.ComponentModel.DataAnnotations;

namespace Users.Api.Models
{
    public class UpdateUserDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Surname { get; set; }
    }
}