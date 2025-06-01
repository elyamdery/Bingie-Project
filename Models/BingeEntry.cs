using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bingie.Models;

public class BingeEntry
{
    public int Id { get; set; } // Primary key
    
    [Required]
    public DateTime Date { get; set; } = DateTime.UtcNow; // Date of the binge
    
    [Required]
    [StringLength(100)]
    public required string Activity { get; set; } // What was binged (e.g., "Netflix", "Gaming", "Social Media")
    
    [StringLength(500)]
    public string? Description { get; set; } // Optional description of the binge session
    
    public TimeSpan Duration { get; set; } // Duration of the binge
    
    [Range(1, 10)]
    public int IntensityRating { get; set; } // How intense was the binge (1-10 scale)
    
    [StringLength(50)]
    public string? Mood { get; set; } // Mood during/after the binge
    
    // Foreign key to User
    [Required]
    public int UserId { get; set; }
    
    // Navigation property to User
    [ForeignKey("UserId")]
    public virtual User User { get; set; } = null!;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // When this entry was created
    
    public DateTime? UpdatedAt { get; set; } // When this entry was last updated
}