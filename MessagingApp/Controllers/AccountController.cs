using MessagingApp.Models;
using MessagingApp.Models.DT0s;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace MessagingApp.Controllers
{

    public class AccountController : BaseApiController
    {
        private DataContext _context;

        public AccountController(DataContext context)
        {
            _context = context;       
        }

        [HttpPost("register")]
        public async Task<ActionResult<AppUser>> Register( RegisterDto register)
        {

            if(await UserExists(register.username))
                return BadRequest("Username is taken");
            
            using var hmac = new HMACSHA512();
            
            var user = new AppUser()
            {
                UserName = register.username.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(register.password)),
                PasswordSalt = hmac.Key
            };
            
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        [HttpPost("login")]
        public async Task<ActionResult<AppUser>> Login(LoginDto login)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == login.Username);
            if (user == null) return Unauthorized("Invalid username/password");
            using var hmac = new HMACSHA512(user.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(login.Password));

            for(int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Inavlid username/password");
            }
            return user;

        }


        private async Task<bool> UserExists(string username)
        {
            return await _context.Users.AnyAsync(x => x.UserName == username.ToLower()); ;
        }
    }
}
