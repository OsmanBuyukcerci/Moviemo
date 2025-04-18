using Microsoft.EntityFrameworkCore;

namespace Moviemo.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Models.User> Users { get; set; }
        public DbSet<Models.Movie> Movies { get; set; }
        public DbSet<Models.Review> Reviews { get; set; }
        public DbSet<Models.Comment> Comments { get; set; }
        public DbSet<Models.Report> Reports { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
