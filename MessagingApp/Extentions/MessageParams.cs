using MessagingApp.Helpers;

namespace MessagingApp.Extentions
{
    public class MessageParams : PaginationParams
    {
        public string? Username { get; set; }
        public string Container { get; set; } = "Unread";
    }
}
