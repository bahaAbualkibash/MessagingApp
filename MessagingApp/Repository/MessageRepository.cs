using AutoMapper;
using AutoMapper.QueryableExtensions;
using MessagingApp.Extentions;
using MessagingApp.Helpers;
using MessagingApp.Models;
using MessagingApp.Models.DT0s;
using Microsoft.EntityFrameworkCore;

namespace MessagingApp.Repository
{
    public class MessageRepository : IMessageRepository
    {
        private DataContext _context;
        private IMapper _mapper;

        public MessageRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async void AddMessage(Message message)
        {
            await _context.Messages.AddAsync(message);
        }

        public void DeleteMessage(Message message)
        {
            _context.Messages.Remove(message);
        }

        public async Task<Message> GetMessage(int id)
        {
            return await _context.Messages
                .Include(m => m.Sender)
                .Include(m => m.Recipient)
                .SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<PagedList<MeesageDto>> GetMessagesForUser(MessageParams messageParams)
        {
            var query = _context.Messages.OrderByDescending(m => m.MessageSent)
            .AsQueryable();

            query = messageParams.Container switch
            {
                "Inbox" => query.Where(u => u.Recipient.UserName == messageParams.Username && u.RecipientDeleted == false),
                "Outbox" => query.Where(u => u.Sender.UserName == messageParams.Username && u.SenderDeleted == false),
                _ => query.Where(u => u.Recipient.UserName == messageParams.Username && u.DateRead == null && u.RecipientDeleted == false)
            };


            var messages = query.ProjectTo<MeesageDto>(_mapper.ConfigurationProvider);

            return await PagedList<MeesageDto>.CreatAsync(messages, messageParams.PageNumber, messageParams.PageSize);

        }

        public async Task<IEnumerable<MeesageDto>> GetMessageThread(string currentUsername, string recipientUsername)
        {
            var messages = await _context.Messages
                .Include( u => u.Sender).ThenInclude( p => p.Photos)
                .Include(u => u.Recipient).ThenInclude(p => p.Photos)
                .Where(m => m.Recipient.UserName == currentUsername && m.RecipientDeleted == false
                    && m.Sender.UserName == recipientUsername
                    || m.Recipient.UserName == recipientUsername
                    && m.Sender.UserName == currentUsername && m.SenderDeleted == false)
                .OrderBy(m => m.MessageSent)
                .ToListAsync();

            var unreadMessaged = messages.Where(m => m.DateRead == null && m.Recipient.UserName == currentUsername).ToList();

            if (unreadMessaged.Any())
            {
                foreach( var message in unreadMessaged)
                {
                    message.DateRead = DateTime.Now;
                }
                await _context.SaveChangesAsync();
            }

            return _mapper.Map<IEnumerable<MeesageDto>>(messages);
         
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
