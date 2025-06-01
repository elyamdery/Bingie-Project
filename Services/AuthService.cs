using Bingie.Data;
using Bingie.Models;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;

namespace Bingie.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDBContext _context;

        public AuthService(AppDBContext context)
        {
            _context = context;
        }        public async Task<User?> RegisterAsync(string username, string password, string email)
        {
            try
            {
                // Validate input
                if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                {
                    return null;
                }

                if (password.Length < 6)
                {
                    return null;
                }

                // Check if user already exists
                var existingUser = await _context.Users
                    .FirstOrDefaultAsync(u => u.Username == username || u.Email == email);

                if (existingUser != null)
                {
                    return null;
                }

                // Hash password securely using BCrypt
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, BCrypt.Net.BCrypt.GenerateSalt(12));

                // Create new user
                var user = new User
                {
                    Username = username,
                    Email = email,
                    Password = hashedPassword,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return user;
            }
            catch (Exception)
            {
                return null;
            }
        }        public async Task<User?> LoginAsync(string username, string password)
        {
            try
            {
                // Validate input
                if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                {
                    return null;
                }

                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Username == username);

                if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
                {
                    return null;
                }

                // Update last login time
                user.LastLoginAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();

                return user;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> IsUsernameTakenAsync(string username)
        {
            try
            {
                return await _context.Users.AnyAsync(u => u.Username == username);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> IsEmailTakenAsync(string email)
        {
            try
            {
                return await _context.Users.AnyAsync(u => u.Email == email);
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}