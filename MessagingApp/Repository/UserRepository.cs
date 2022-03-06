using AutoMapper;
using AutoMapper.QueryableExtensions;
using MessagingApp.Extentions;
using MessagingApp.Helpers;
using MessagingApp.Models;
using MessagingApp.Models.DT0s;
using Microsoft.EntityFrameworkCore;

namespace MessagingApp.Repository
{
    public class UserRepository : IUserRepository
    {
        private DataContext _context;
        private IMapper _mapper;

        public UserRepository(DataContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<MemberDto> GetMemberAsync(string name)
        {
            return await _context.Users
                .Where(u => u.UserName == name)
                .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();
        }

        public async Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams)
        {
            var query = _context.Users.AsQueryable();
            query = query.Where(u => u.UserName != userParams.CurrentUsername);

            query = userParams.OrderBy switch
            {
                "created" => query.OrderByDescending(u => u.created),
                _ => query.OrderByDescending(u => u.LastActive)
            };

            return await PagedList<MemberDto>.CreatAsync(
                query.ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
                .AsNoTracking(),
                userParams.PageNumber,
                userParams.PageSize);
        
        }
        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<AppUser> GetUserByUsernameAsync(string name)
        {
            return await _context.Users
                .Include(p => p.Photos)
                .SingleOrDefaultAsync(x => x.UserName == name);
        }

        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            return await _context.Users
                .Include(p => p.Photos)
                .ToListAsync();
        }
         
        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async void Update(AppUser user)
        {
            _context.Entry(user).State = EntityState.Modified;
        }
    }
}
