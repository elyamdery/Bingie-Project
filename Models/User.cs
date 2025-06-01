using System.ComponentModel.DataAnnotations;

namespace Bingie.Models;

public class User
{
    public int Id { get; set; } // Primary key
    
    [Required]
    [StringLength(50, MinimumLength = 3)]
    public required string Username { get; set; } // User's username
    
    [Required]
    [EmailAddress]
    [StringLength(100)]
    public required string Email { get; set; } // User's email address
    
    [Required]
    [StringLength(255)]
    public required string Password { get; set; } // Hashed password for secure storage
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Account creation timestamp
    
    public DateTime? LastLoginAt { get; set; } // Last login timestamp
    
    // Navigation property for user's binge entries
    public virtual ICollection<BingeEntry> BingeEntries { get; set; } = new List<BingeEntry>();
}