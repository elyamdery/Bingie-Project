using Bingie.Models;
using Bingie.Services; // Add this using directive

public class AuthService : IAuthService
{
    private readonly IDataStore<User> _databaseService;

    public AuthService(IDataStore<User> databaseService)
    {
        _databaseService = databaseService;
    }

    public async Task<bool> LoginAsync(string username, string password)
    {
        IEnumerable<User> users = await _databaseService.GetItemsAsync();
        User? user = users.FirstOrDefault(u => u.Username == username && u.Password == password);
        return user != null;
    }

    public async Task<bool> RegisterAsync(string username, string password)
    {
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            return false;
        }

        IEnumerable<User> users = await _databaseService.GetItemsAsync();
        if (users.Any(u => u.Username == username))
        {
            return false;
        }

        User newUser = new()
        {
            Username = username,
            Password = password
        };

        _ = await _databaseService.AddItemAsync(newUser);
        return true;
    }

    public async Task LogoutAsync()
    {
        // Clear any session or authentication token
        await Task.CompletedTask;
    }
}