using MessagingApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MessagingApp.Controllers
{

    public class UsersController : BaseApiController
    {
        private DataContext _context;

        public UsersController(DataContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUser>>> getUsers()
        {
            var users = await _context.Users.ToListAsync();
            return Ok(users);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<AppUser>> getUser(int id)
        {
            return await _context.Users.FindAsync(id);
           
        }
    }
}
