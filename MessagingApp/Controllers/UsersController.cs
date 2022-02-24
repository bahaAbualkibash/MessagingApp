using AutoMapper;
using MessagingApp.Models;
using MessagingApp.Models.DT0s;
using MessagingApp.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MessagingApp.Controllers
{
    [Authorize]
    public class UsersController : BaseApiController
    {
        private IUserRepository _userRepo;
        private IMapper _mapper;

        public UsersController(IUserRepository userRepository,
            IMapper mapper)
        {
            _userRepo = userRepository;
            _mapper = mapper;
        }

        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> getUsers()
        {
            var users = await _userRepo.GetUsersAsync();
            var usersToReturn = _mapper.Map<IEnumerable<MemberDto>>(users);
            return Ok(usersToReturn);
        }

        [HttpGet("{username}")]
        public async Task<ActionResult<MemberDto>> getUser(string username)
        {
            
            return  await _userRepo.GetMemberAsync(username);


        }
    }
}
