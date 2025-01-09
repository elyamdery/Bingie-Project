using Microsoft.EntityFrameworkCore;
using Bingie.Models;

namespace Bingie.Data
{
    public class AppDbContext : DbContext
    {
        private readonly string _connectionString;

        public AppDbContext(string connectionString)
        {
            _connectionString = connectionString;
            Database.EnsureCreated();
        }

        public DbSet<User> Users { get; set; }
        public DbSet<BingeEntry> BingeEntries { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Ensure usernames are unique
            modelBuilder.Entity<User>().HasIndex(u => u.Username).IsUnique();
        }
    }
}