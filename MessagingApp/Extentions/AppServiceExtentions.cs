using MessagingApp.Interfaces;
using MessagingApp.Models;
using MessagingApp.Services;
using Microsoft.EntityFrameworkCore;

namespace MessagingApp.Extentions
{
    public static class AppServiceExtentions
    {
        public static IServiceCollection AddAppServices(this IServiceCollection services, WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            return services;
        }
    }
}
