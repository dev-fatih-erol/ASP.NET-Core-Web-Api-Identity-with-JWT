using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Users.Api.Data.Entities;
using Users.Api.Infrastructure.Extensions;
using Users.Api.Infrastructure.Helpers;
using Users.Api.Models;
using Users.Api.Services;

namespace Users.Api.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IEmailSender _emailSender;

        public AccountController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
        }

        [HttpGet]
        [Route("Account/ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            return Ok();
        }

        [HttpPost]
        [Route("Account/Register")]
        public async Task<IActionResult> Register([FromBody]RegisterDto request)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    Name = request.Name,
                    Surname = request.Surname,
                    UserName = request.UserName,
                    Email = request.Email
                };
                var result = await _userManager.CreateAsync(user, request.Password);
                if (result.Succeeded)
                {
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = UrlBuilder.EmailConfirmationLink(user.Id, code);
                    await _emailSender.SendEmailConfirmationAsync(request.Email, callbackUrl);

                    return Created($"User/{user.Id}", user);
                }
                ModelState.AddErrors(result);
            }

            return BadRequest(ModelState);
        }
    }
}