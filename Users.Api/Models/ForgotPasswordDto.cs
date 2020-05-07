using System.ComponentModel.DataAnnotations;

namespace Users.Api.Models
{
    public class ForgotPasswordDto
    {
        [Required]
        public string Email { get; set; }
    }
}