using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Users.Api.Data.Entities;
using Users.Api.Infrastructure.Helpers;

namespace Users.Api.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class MeController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly Mapper _mapper;

        public MeController(
            UserManager<User> userManager,
            Mapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            return Ok(_mapper.MapToMeDto(user));
        }
    }
}