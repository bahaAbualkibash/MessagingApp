using MessagingApp.Models;

namespace MessagingApp.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);
    }
}
