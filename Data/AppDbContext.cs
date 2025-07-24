using Microsoft.EntityFrameworkCore;
using QuantSignalServer.Models;

namespace QuantSignalServer.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Strategy> Strategies { get; set; }
        public DbSet<Signal> Signals { get; set; }
        public DbSet<ForwardTarget> ForwardTargets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Strategy>()
                .HasKey(s => s.Id);

            modelBuilder.Entity<Strategy>()
                .HasIndex(s => s.Name)
                .IsUnique();

            // 这里 UserId 是 string 类型
            modelBuilder.Entity<Strategy>()
                .HasOne(s => s.User)
                .WithMany()
                .HasForeignKey(s => s.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}