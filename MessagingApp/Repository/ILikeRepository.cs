using MessagingApp.Extentions;
using MessagingApp.Helpers;
using MessagingApp.Models;
using MessagingApp.Models.DT0s;

namespace MessagingApp.Repository
{
    public interface ILikeRepository
    {
        Task<UserLike> GetUserLike(int sourceUserId, int likedUserId);
        Task<AppUser> GetUserWithLikes(int userId);
        Task<PagedList<LikeDto>> GetUserLikes(LikesParams likesParams);
    }
}
