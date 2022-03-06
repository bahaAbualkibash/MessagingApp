using MessagingApp.Helpers;

namespace MessagingApp.Extentions
{
    public class LikesParams : PaginationParams
    {
        public string Predicate { get; set; }
        public int UserId { get; set; }

    }
}
