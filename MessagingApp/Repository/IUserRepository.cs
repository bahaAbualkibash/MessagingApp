using MessagingApp.Extentions;
using MessagingApp.Helpers;
using MessagingApp.Models;
using MessagingApp.Models.DT0s;

namespace MessagingApp.Repository
{
    public interface IUserRepository
    {
        void Update(AppUser user);
        Task<bool> SaveAllAsync();
        Task<IEnumerable<AppUser>> GetUsersAsync();
        Task<AppUser> GetUserByIdAsync(int id);
        Task<AppUser> GetUserByUsernameAsync(string name);
        Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams);
        Task<MemberDto> GetMemberAsync(string name);
    }
}
