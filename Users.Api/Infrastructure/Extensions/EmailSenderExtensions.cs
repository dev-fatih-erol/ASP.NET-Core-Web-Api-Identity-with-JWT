﻿using System.Threading.Tasks;
using Users.Api.Services;

namespace Users.Api.Infrastructure.Extensions
{
    public static class EmailSenderExtensions
    {
        public static Task SendEmailConfirmationAsync1(this IEmailSender emailSender, string email, string link)
        {
            return emailSender.SendEmailAsync(email, "Reset Password",
                $"Please reset your password by clicking here: <a href='{link}'>link</a>");
        }

        public static Task SendEmailConfirmationAsync(this IEmailSender emailSender, string email, string link)
        {
            return emailSender.SendEmailAsync(email, "Confirm your email",
                $"Please confirm your account by clicking this link: <a href='{link}'>link</a>");
        }
    }
}