namespace Bingie.Models;

public class BingeEntry
{
    public int Id { get; set; } // Primary key
    public DateTime Date { get; set; } // Date of the binge
    public required string Username { get; set; } // Username of the person who binged
    public TimeSpan Duration { get; set; } // Duration of the binge -----> Remove in the future | Not Needed*
}