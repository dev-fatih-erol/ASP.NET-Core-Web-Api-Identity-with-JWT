using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
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
        private readonly IConfiguration _configuration;

        public AccountController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IEmailSender emailSender,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _configuration = configuration;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("Account/Login")]
        public async Task<IActionResult> Login([FromBody]LoginDto request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user != null)
            {
                var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
                if (result.Succeeded)
                {
                    var date = DateTime.UtcNow;
                    var claims = new[]
                        {
                            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                            new Claim(JwtRegisteredClaimNames.Iat, date.ToUniversalTime().ToString(), ClaimValueTypes.Integer64)
                        };
                    var securityKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(_configuration["JwtConfiguration:SecurityKey"]));
                    var securityToken = new JwtSecurityToken(
                        issuer: _configuration["JwtConfiguration:Issuer"],
                        audience: _configuration["JwtConfiguration:Audience"],
                        claims: claims,
                        notBefore: date,
                        expires: date.AddMinutes(60),
                        signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)
                    );

                    var token = new JwtSecurityTokenHandler().WriteToken(securityToken);
                    return Ok(token);
                }
            }

            return BadRequest("Please try another email address or password.");
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("Account/ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody]ResetPasswordDto request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return BadRequest("Please try another email address.");
            }

            var result = await _userManager.ResetPasswordAsync(user, request.Code, request.Password);
            if (result.Succeeded)
            {
                return Ok("Your password has been reset.");
            }

            return BadRequest(result.GetError());
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("Account/ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody]ForgotPasswordDto request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null || !await _userManager.IsEmailConfirmedAsync(user))
            {
                return BadRequest("Please try another email address.");
            }

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            var callbackUrl = UrlBuilder.ResetPasswordCallbackLink(code);
            await _emailSender.SendEmailAsync(request.Email, "Reset Password",
                $"Please reset your password by clicking here: <a href='{callbackUrl}'>link</a>");

            return Ok("Please check your email to reset your password.");
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("Account/ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail([FromQuery]int userId, [FromQuery]string code)
        {
            if (code == null)
            {
                throw new ApplicationException("A code must be supplied for email confirm.");
            }

            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{userId}'.");
            }

            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
            {
                return Ok("Thank you for confirming your email.");
            }
            
            return BadRequest(result.GetError());
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("Account/Register")]
        public async Task<IActionResult> Register([FromBody]RegisterDto request)
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
                var callbackUrl = UrlBuilder.EmailConfirmationLink(user.Id, HttpUtility.UrlEncode(code));
                await _emailSender.SendEmailAsync(request.Email, "Confirm your email",
                    $"Please confirm your account by clicking this link: <a href='{callbackUrl}'>link</a>");

                return Created($"User/{user.Id}", null);
            }

            return BadRequest(result.GetError());
        }
    }
}