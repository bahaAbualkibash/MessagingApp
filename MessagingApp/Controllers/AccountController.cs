using AutoMapper;
using MessagingApp.Interfaces;
using MessagingApp.Models;
using MessagingApp.Models.DT0s;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace MessagingApp.Controllers
{

    public class AccountController : BaseApiController
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private ITokenService _tokenService;
        private IMapper _mapper;
        public AccountController(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager, ITokenService tokenService,IMapper mapper)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register( RegisterDto register)
        {

            if(await UserExists(register.username))
                return BadRequest("Username is taken");
            var user = _mapper.Map<AppUser>(register);


            user.UserName = register.username.ToLower();


            var result = await userManager.CreateAsync(user, register.password);

            if (!result.Succeeded) return BadRequest(result.Errors);

            var roleResult = await userManager.AddToRoleAsync(user, "Member");

            if (!roleResult.Succeeded) return BadRequest(roleResult.Errors);

            return new UserDto()
            {
                Token = await _tokenService.CreateToken(user),
                Username = user.UserName,
                KnownAs = user.KnownAs
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto login)
        {
            var user = await userManager.Users
                .Include(p => p.Photos)
                .SingleOrDefaultAsync(x => x.UserName == login.Username.ToLower());
            
            if (user == null) return Unauthorized("Invalid username/password");

            var result = await signInManager.CheckPasswordSignInAsync(user,login.Password,false);

            if(!result.Succeeded) return Unauthorized();


            return new UserDto()
            {
                Token = await _tokenService.CreateToken(user),
                Username = user.UserName,
                photoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url,
                KnownAs  =user.KnownAs
            };

        }


        private async Task<bool> UserExists(string username)
        {
            return await userManager.Users.AnyAsync(x => x.UserName == username.ToLower()); ;
        }
    }
}
