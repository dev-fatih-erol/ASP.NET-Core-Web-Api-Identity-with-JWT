using System.ComponentModel.DataAnnotations;

namespace Users.Api.Models
{
    public class ForgotPasswordDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}