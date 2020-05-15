using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Users.Api.Data.Entities;
using Users.Api.Infrastructure.Extensions;
using Users.Api.Infrastructure.Helpers;
using Users.Api.Models;

namespace Users.Api.Controllers
{
    public class ManageController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly Mapper _mapper;

        public ManageController(
            UserManager<User> userManager,
            Mapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        [HttpPost]
        [Authorize]
        [Route("Manage/ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody]ChangePasswordDto request)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var result = await _userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);
            if (result.Succeeded)
            {
                return Ok("Your password has been changed.");
            }

            return BadRequest(result.GetError());
        }

        [HttpGet]
        [Authorize]
        [Route("Manage")]
        public async Task<IActionResult> Get()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            return Ok(_mapper.MapToUserDto(user));
        }
    }
}