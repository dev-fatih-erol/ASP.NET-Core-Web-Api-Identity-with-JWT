using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        [HttpPost]
        [AllowAnonymous]
        [Route("Account/ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody]ResetPasswordDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest("Error1");

            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                return BadRequest("Error2");

            var result = await _userManager.ResetPasswordAsync(user, request.Code, request.Password);
            if (result.Succeeded)
                return Ok(true);

            ModelState.AddErrors(result);

            return BadRequest(ModelState);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("Account/ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody]ForgotPasswordDto request)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(request.Email);
                if (user == null || !await _userManager.IsEmailConfirmedAsync(user))
                    return BadRequest("Error1");

                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = UrlBuilder.ResetPasswordCallbackLink(code);
                await _emailSender.SendEmailAsync(request.Email, "Reset Password",
                    $"Please reset your password by clicking here: <a href='{callbackUrl}'>link</a>");

                return Ok(true);
            }

            return BadRequest(ModelState);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("Account/ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail([FromQuery]int userId, [FromQuery]string code)
        {
            if (userId <= 0 || code == null)
                return BadRequest("Error1");

            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
                return BadRequest("Error2");

            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (!result.Succeeded)
                return BadRequest("Error3");

            return Ok(result.Succeeded);
        }

        [HttpPost]
        [AllowAnonymous]
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
                    code = HttpUtility.UrlEncode(code);
                    var callbackUrl = UrlBuilder.EmailConfirmationLink(user.Id, code);
                    await _emailSender.SendEmailAsync(request.Email, "Confirm your email",
                        $"Please confirm your account by clicking this link: <a href='{callbackUrl}'>link</a>");

                    var response = Mapper.MapToUserDto(user);
                    return Created($"User/{user.Id}", response);
                }
                ModelState.AddErrors(result);
            }

            return BadRequest(ModelState);
        }
    }
}