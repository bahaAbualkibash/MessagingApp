using Microsoft.EntityFrameworkCore;

namespace MessagingApp.Models
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        { 
        }

        public DbSet<AppUser> Users { get; set; }
    }
}
