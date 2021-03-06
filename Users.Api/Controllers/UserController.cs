﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Users.Api.Data.Entities;
using Users.Api.Infrastructure.Extensions;
using Users.Api.Infrastructure.Helpers;
using Users.Api.Models;

namespace Users.Api.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly Mapper _mapper;

        public UserController(
            UserManager<User> userManager,
            Mapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        [HttpPut]
        [Route("User/Password")]
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
                return NoContent();
            }

            return BadRequest(result.GetError());
        }

        [HttpPut]
        [Route("User")]
        public async Task<IActionResult> Update([FromBody]UpdateUserDto request)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            user.Name = request.Name;
            user.Surname = request.Surname;
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return NoContent();
            }

            return BadRequest(result.GetError());
        }

        [HttpGet]
        [Route("User/{id:int}")]
        public async Task<IActionResult> GetById([FromRoute]int id)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(_mapper.MapToUserDto(user));
        }
    }
}