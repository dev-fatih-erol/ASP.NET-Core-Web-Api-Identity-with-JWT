using System;
using Microsoft.AspNetCore.Mvc;
using Users.Api.Controllers;

namespace Users.Api.Infrastructure.Extensions
{
    public static class UrlExtensions
    {
        private const string ApplicationUrl = "https://localhost:5001";

        public static string GenerateUrl(this IUrlHelper urlHelper, string contentPath)
        {
            var request = urlHelper.ActionContext.HttpContext.Request;
            var content = urlHelper.Content(contentPath);
            return new Uri(new Uri($"{request.Scheme}://{request.Host.Value}"), content).ToString();
        }

        public static string EmailConfirmationLink(this IUrlHelper urlHelper, int userId, string code, string scheme)
        {
            return urlHelper.Action(
                action: nameof(AccountController.ConfirmEmail),
                controller: "Account",
                values: new { userId, code },
                protocol: scheme);
        }
    }
}