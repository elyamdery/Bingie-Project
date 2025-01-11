public interface IAuthService
{
    // Authenticates the user with the provided credentials (username & password).
    // Returns true if authentication is successful, otherwise false.
    Task<bool> LoginAsync(string username, string password);

    // Logs out the current user.
    // Clears any session or authentication token.
    Task LogoutAsync();

    // Registers a new user with the provided username and password.
    // Returns true if registration is successful, otherwise false.
    Task<bool> RegisterAsync(string username, string password);
}