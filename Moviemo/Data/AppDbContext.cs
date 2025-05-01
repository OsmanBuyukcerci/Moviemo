using Microsoft.EntityFrameworkCore;
using Moviemo.Models;

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
        public DbSet<Models.Vote> Votes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Models.Comment>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Models.Comment>()
                .HasOne<Movie>()
                .WithMany()
                .HasForeignKey(m => m.MovieId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Models.Report>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Models.Review>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Models.Review>()
                .HasOne<Movie>()
                .WithMany()
                .HasForeignKey(m => m.MovieId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
