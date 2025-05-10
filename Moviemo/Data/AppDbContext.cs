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
                .HasOne(C => C.User)
                .WithMany(U => U.Comments)
                .HasForeignKey(U => U.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Models.Comment>()
                .HasOne(C => C.Movie)
                .WithMany(M => M.Comments)
                .HasForeignKey(C => C.MovieId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Models.Report>()
                .HasOne(R => R.User)
                .WithMany(U => U.Reports)
                .HasForeignKey(R => R.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Models.Review>()
                .HasOne(R => R.User)
                .WithMany(U => U.Reviews)
                .HasForeignKey(R => R.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Models.Review>()
                .HasOne(R => R.Movie)
                .WithMany(M => M.Reviews)
                .HasForeignKey(R => R.MovieId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Models.Vote>()
                .HasOne(V => V.User)
                .WithMany(U => U.Votes)
                .HasForeignKey(V => V.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Models.Vote>()
                .HasOne(V => V.Comment)
                .WithMany(C => C.Votes)
                .HasForeignKey(V => V.CommentId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
