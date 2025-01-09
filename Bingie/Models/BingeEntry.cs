using System;

namespace Bingie.Models
{
    public class BingeEntry
    {
        public int Id { get; set; } // Primary key
        public DateTime Date { get; set; } // Date of the binge
        public string Username { get; set; } // Username of the person who binged
        public TimeSpan Duration { get; set; } // Duration of the binge
    }

    public class User
    {
        public int Id { get; set; } // Primary key
        public string Username { get; set; } // User's username
        public string Password { get; set; } // Hashed password for secure storage
    }
}