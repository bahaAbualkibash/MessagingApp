using MessagingApp.Helpers;
using MessagingApp.Interfaces;
using MessagingApp.Models;
using MessagingApp.Repository;
using MessagingApp.Services;
using Microsoft.EntityFrameworkCore;

namespace MessagingApp.Extentions
{
    public static class AppServiceExtentions
    {
        public static IServiceCollection AddAppServices(this IServiceCollection services, WebApplicationBuilder builder)
        {
            builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));
            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            builder.Services.AddScoped<LogUserActivity>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);
            builder.Services.AddScoped<IPhotoService,PhotoSerivce>();
           return services;
        }
    }
}
