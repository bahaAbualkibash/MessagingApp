using MessagingApp.Extentions;
using MessagingApp.Repository;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MessagingApp.Helpers
{
    public class LogUserActivity : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var resultContext = await next();

            if (!resultContext.HttpContext.User.Identity.IsAuthenticated) return;

            var id = resultContext.HttpContext.User.GetUserId();
            var repo = resultContext.HttpContext.RequestServices.GetService<IUserRepository>();
            var user = await repo.GetUserByIdAsync(id);
            user.LastActive = DateTime.Now;
            await repo.SaveAllAsync();
        }
    }
}
