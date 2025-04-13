namespace Bingie.Models;

public class BingeEntry
{
    public int Id { get; set; } // Primary key
    public DateTime Date { get; set; } // Date of the binge
    public required string Username { get; set; } // Username of the person who binged
    public TimeSpan Duration { get; set; } // Duration of the binge (kept for backward compatibility)
    public bool WasUrgeResisted { get; set; } // Whether the user resisted the urge (for urge surfing feature)
    public int Points { get; set; } // Points earned for this entry (for reward system)
    public string Notes { get; set; } = string.Empty; // Optional notes about the binge episode

    // Helper method to calculate points based on various factors
    public static int CalculatePoints(bool wasUrgeResisted, int consecutiveDaysWithoutBinge)
    {
        int points = 0;

        // Points for resisting an urge
        if (wasUrgeResisted)
        {
            points += 10; // Base points for resisting

            // Bonus points based on streak
            if (consecutiveDaysWithoutBinge > 0)
            {
                points += Math.Min(consecutiveDaysWithoutBinge * 2, 20); // Up to 20 bonus points based on streak
            }
        }
        else
        {
            // Still give some points for tracking even if they didn't resist
            points += 1;
        }

        return points;
    }
}