using MessagingApp.Extentions;
using MessagingApp.Models.DT0s;
using MessagingApp.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MessagingApp.Controllers
{
    [Authorize]
    public class LikesController : BaseApiController
    {
        private ILikeRepository _likesRepo;
        private IUserRepository _userRepo;

        public LikesController(IUserRepository userRepository,ILikeRepository likeRepository)
        {
            _likesRepo = likeRepository;
            _userRepo = userRepository;
        }

        [HttpPost("{username}")]
        public async Task<ActionResult> AddLike(string username)
        {
            var sourceUserId = User.GetUserId();
            var likedUser = await _userRepo.GetUserByUsernameAsync(username); ;
            var sourceUser = await _likesRepo.GetUserWithLikes(sourceUserId);

            if(likedUser == null) return NotFound();

            if (sourceUser.UserName == username) return BadRequest();

            var userLike = await _likesRepo.GetUserLike(sourceUserId, likedUser.Id) ;

            if (userLike != null) return BadRequest("You already like this user");

            userLike = new UserLike
            {
                SourceUserId = sourceUserId,
                LikedUserId = likedUser.Id,
            };
            sourceUser.LikedUsers.Add(userLike);
            if (await _userRepo.SaveAllAsync()) return Ok();
            return BadRequest("Failed to like user");
        }

        [HttpGet]
        public async Task<ActionResult<LikeDto>> GetUserLikes([FromQuery]LikesParams likesParams)
        {
            likesParams.UserId = User.GetUserId();
            var users =  await _likesRepo.GetUserLikes(likesParams);
            Response.AddPaginationHeader(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages);
            return Ok(users);
        }
    }
}
