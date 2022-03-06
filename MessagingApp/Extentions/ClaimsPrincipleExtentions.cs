using System.Security.Claims;

namespace MessagingApp.Extentions
{
    public static class ClaimsPrincipleExtentions
    {
         public static string GetUsername(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.Name)?.Value;

        }
        public static int GetUserId(this ClaimsPrincipal user)
        {
            var value = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.Parse(value);

        }
    }
}
