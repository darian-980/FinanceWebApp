using Microsoft.EntityFrameworkCore;
using GroupProject.Models;

namespace GroupProject.Models
{
    public class UserContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public UserContext(DbContextOptions options) : base(options) 
        {
        }
        public DbSet<GroupProject.Models.Account> Account { get; set; } = default!;
        public DbSet<GroupProject.Models.Transaction> Transaction { get; set; } = default!;

        public DbSet<GroupProject.Models.Menu> Menu { get; set; } = default!;
    }
}
