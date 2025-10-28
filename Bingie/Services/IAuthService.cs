using Bingie.Models;

namespace Bingie.Services;

public interface IAuthService
{
    // Authenticates a user with username & password, returning the matching user when successful.
    Task<User?> LoginAsync(string username, string password);

    // Authenticates via a persisted remember-me token.
    Task<User?> LoginWithTokenAsync(string username, string token);

    // Issues a fresh remember-me token for the user, returning the plaintext token to persist securely on device.
    Task<string?> IssueRememberTokenAsync(User user);

    // Clears any stored remember-me token for the user.
    Task ClearRememberTokenAsync(User user);

    // Logs out the current user.
    // Clears any session or authentication token.
    Task LogoutAsync();

    // Registers a new user with the provided username and password.
    // Returns true if registration is successful, otherwise false.
    Task<bool> RegisterAsync(string username, string password);
}
