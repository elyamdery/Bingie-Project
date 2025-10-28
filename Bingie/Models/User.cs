namespace Bingie.Models;

public class User
{
    public int Id { get; set; } // Primary key
    public required string Username { get; set; } // User's username
    public required string Password { get; set; } // Hashed password for secure storage
    public string? RememberToken { get; set; } // Optional hashed remember-me token
}
