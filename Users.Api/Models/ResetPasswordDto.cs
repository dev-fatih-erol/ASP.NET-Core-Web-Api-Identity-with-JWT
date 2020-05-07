using System.ComponentModel.DataAnnotations;

namespace Users.Api.Models
{
    public class ResetPasswordDto
    {
        public string Code { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string ConfirmPassword { get; set; }
    }
}