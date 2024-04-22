using Microsoft.EntityFrameworkCore;
using shared.Model; // Sørg for at denne linje er til stede for at inkludere dine modeller

namespace web_api.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<User> Users { get; set; }

        // Tilføj denne konstruktør
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
    }
}