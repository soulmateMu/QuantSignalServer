using Microsoft.EntityFrameworkCore;
using QuantSignalServer.Models;

namespace QuantSignalServer.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Strategy> Strategies { get; set; }
        public DbSet<Signal> Signals { get; set; }
        public DbSet<ForwardTarget> ForwardTargets { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Strategy>()
                .HasKey(s => s.Id);
            modelBuilder.Entity<Strategy>()
                .HasIndex(s => s.Name)
                .IsUnique();
            modelBuilder.Entity<Strategy>()
                .HasOne(s => s.User)
                .WithMany()
                .HasForeignKey(s => s.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ForwardTarget>()
                .HasKey(ft => ft.Id);
            modelBuilder.Entity<ForwardTarget>()
                .HasOne<Strategy>()
                .WithMany(s => s.ForwardTargets)
                .HasForeignKey(ft => ft.StrategyId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}