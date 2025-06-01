using Bingie.Models;
using Microsoft.EntityFrameworkCore;

namespace Bingie.Data;

public class AppDBContext : DbContext
{
    public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<BingeEntry> BingeEntries { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User configuration
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.HasIndex(u => u.Username).IsUnique();
            entity.HasIndex(u => u.Email).IsUnique();
            
            entity.Property(u => u.Username)
                .IsRequired()
                .HasMaxLength(50);
                
            entity.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(100);
                
            entity.Property(u => u.Password)
                .IsRequired()
                .HasMaxLength(255);
                
            entity.Property(u => u.CreatedAt)
                .IsRequired();
        });

        // BingeEntry configuration
        modelBuilder.Entity<BingeEntry>(entity =>
        {
            entity.HasKey(b => b.Id);
            
            entity.Property(b => b.Activity)
                .IsRequired()
                .HasMaxLength(100);
                
            entity.Property(b => b.Description)
                .HasMaxLength(500);
                
            entity.Property(b => b.Mood)
                .HasMaxLength(50);
                
            entity.Property(b => b.Date)
                .IsRequired();
                
            entity.Property(b => b.CreatedAt)
                .IsRequired();

            // Configure relationship with User
            entity.HasOne(b => b.User)
                .WithMany(u => u.BingeEntries)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}