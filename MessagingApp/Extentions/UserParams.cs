using MessagingApp.Helpers;

namespace MessagingApp.Extentions
{
    public class UserParams : PaginationParams
    {
      
        public string? CurrentUsername { get; set; }
        public string OrderBy { get; set; } = "lastActive";
    }
}
