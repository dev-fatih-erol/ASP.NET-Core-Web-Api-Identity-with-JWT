using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Users.Api.Data.Entities;

namespace Users.Api.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<User> _userManager;

        public UserController(
            UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        [Route("User/{id:int}")]
        public async Task<IActionResult> GetById([FromRoute]int id)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (user != null)
            {
                return Ok(user);
            }

            return NotFound();
        }
    }
}