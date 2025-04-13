using Bingie.Models;
using Bingie.Services;
using Serilog;

namespace Bingie.Services;

public class AuthService : IAuthService
{
    private readonly IDataStore<User> _databaseService;
    private readonly PasswordHasher _passwordHasher;
    private readonly TokenService _tokenService;

    public AuthService(IDataStore<User> databaseService)
    {
        _databaseService = databaseService;
        _passwordHasher = new PasswordHasher();
        _tokenService = new TokenService();
    }

    public async Task<bool> LoginAsync(string username, string password)
    {
        try
        {
            Log.Information("Attempting login for user: {Username}", username);

            // Get all users
            IEnumerable<User> users = await _databaseService.GetItemsAsync();
            var user = users.FirstOrDefault(u => u.Username == username);

            if (user == null)
            {
                Log.Information("Login failed: User {Username} not found", username);
                return false;
            }

            // Check if this is a legacy password (not hashed)
            if (!user.Password.Contains(':'))
            {
                // Legacy password - direct comparison
                if (user.Password != password)
                {
                    Log.Information("Login failed: Invalid password for user {Username}", username);
                    return false;
                }

                // Update to hashed password for future logins
                user.Password = _passwordHasher.Hash(password);
                await _databaseService.UpdateItemAsync(user);
                Log.Information("Updated legacy password to secure hash for user {Username}", username);
            }
            else
            {
                // Verify password hash
                if (!_passwordHasher.Verify(user.Password, password))
                {
                    Log.Information("Login failed: Invalid password for user {Username}", username);
                    return false;
                }
            }

            // Generate authentication token
            _ = _tokenService.GenerateToken(username);
            Log.Information("Login successful for user {Username}", username);
            return true;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error during login for user {Username}", username);
            return false;
        }
    }

    public async Task<bool> RegisterAsync(string username, string password)
    {
        try
        {
            Log.Information("Attempting to register new user: {Username}", username);

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                Log.Information("Registration failed: Empty username or password");
                return false;
            }

            // Check if username already exists
            IEnumerable<User> users = await _databaseService.GetItemsAsync();
            if (users.Any(u => u.Username == username))
            {
                Log.Information("Registration failed: Username {Username} already exists", username);
                return false;
            }

            // Hash the password
            string hashedPassword = _passwordHasher.Hash(password);

            // Create new user with hashed password
            User newUser = new()
            {
                Username = username,
                Password = hashedPassword
            };

            // Add user to database
            _ = await _databaseService.AddItemAsync(newUser);
            Log.Information("Registration successful for user {Username}", username);

            // Generate authentication token
            _ = _tokenService.GenerateToken(username);
            return true;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error during registration for user {Username}", username);
            return false;
        }
    }

    public async Task LogoutAsync()
    {
        try
        {
            var username = _tokenService.GetCurrentUsername();
            _tokenService.ClearToken();
            Log.Information("Logout successful for user {Username}", username);
            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error during logout");
        }
    }

    public bool IsAuthenticated()
    {
        return _tokenService.ValidateToken();
    }

    public string GetCurrentUsername()
    {
        return _tokenService.GetCurrentUsername();
    }
}