using Bingie.Models;
using Microsoft.EntityFrameworkCore;

namespace Bingie.Data
{
    public class AppDbContext : DbContext
    {
        private readonly string _connectionString;

        public AppDbContext(string connectionString)
        {
            _connectionString = connectionString;
            _ = Database.EnsureCreated();
        }

        public DbSet<User> Users { get; set; }
        public DbSet<BingeEntry> BingeEntries { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            _ = optionsBuilder.UseSqlite(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Ensure usernames are unique
            _ = modelBuilder.Entity<User>().HasIndex(u => u.Username).IsUnique();
        }
    }
}