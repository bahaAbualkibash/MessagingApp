using MessagingApp.Extentions;
using MessagingApp.Helpers;
using MessagingApp.Models;
using MessagingApp.Models.DT0s;

namespace MessagingApp.Repository
{
    public interface IMessageRepository
    {
        void AddMessage(Message message);
        void DeleteMessage(Message message);
        Task<Message> GetMessage(int id);
        Task<PagedList<MeesageDto>> GetMessagesForUser(MessageParams messageParams);
        Task<IEnumerable<MeesageDto>> GetMessageThread(string currentUsername,string recipientUsername);
        Task<bool> SaveAllAsync();
    }
}
