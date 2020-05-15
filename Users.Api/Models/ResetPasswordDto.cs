﻿using System.ComponentModel.DataAnnotations;

namespace Users.Api.Models
{
    public class ResetPasswordDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; }

        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }
}