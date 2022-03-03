using AutoMapper;
using MessagingApp.Interfaces;
using MessagingApp.Models;
using MessagingApp.Models.DT0s;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace MessagingApp.Controllers
{

    public class AccountController : BaseApiController
    {
        private DataContext _context;
        private ITokenService _tokenService;
        private IMapper _mapper;
        public AccountController(DataContext context, ITokenService tokenService,IMapper mapper)
        {
            _context = context;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register( RegisterDto register)
        {

            if(await UserExists(register.username))
                return BadRequest("Username is taken");
            var user = _mapper.Map<AppUser>(register);
            using var hmac = new HMACSHA512();


            user.UserName = register.username.ToLower();
            user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(register.password));
                user.PasswordSalt = hmac.Key;
            
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new UserDto()
            {
                Token = _tokenService.CreateToken(user),
                Username = user.UserName,
                KnownAs = user.KnownAs
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto login)
        {
            var user = await _context.Users
                .Include(p => p.Photos)
                .SingleOrDefaultAsync(x => x.UserName == login.Username);
            
            if (user == null) return Unauthorized("Invalid username/password");
            
            using var hmac = new HMACSHA512(user.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(login.Password));

            for(int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Inavlid username/password");
            }

            return new UserDto()
            {
                Token = _tokenService.CreateToken(user),
                Username = user.UserName,
                photoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url,
                KnownAs  =user.KnownAs
            };

        }


        private async Task<bool> UserExists(string username)
        {
            return await _context.Users.AnyAsync(x => x.UserName == username.ToLower()); ;
        }
    }
}
