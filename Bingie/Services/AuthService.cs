using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Bingie.Models;
using BCryptNet = BCrypt.Net.BCrypt;

namespace Bingie.Services;

public class AuthService : IAuthService
{
    private readonly IDataStore<User> _databaseService;

    public AuthService(IDataStore<User> databaseService)
    {
        _databaseService = databaseService;
    }

    public async Task<User?> LoginAsync(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password)) return null;

        var normalizedPassword = password.Trim();

        var user = await FindUserAsync(username);
        if (user == null) return null;

        return BCryptNet.Verify(normalizedPassword, user.Password) ? user : null;
    }

    public async Task<User?> LoginWithTokenAsync(string username, string token)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(token)) return null;

        var user = await FindUserAsync(username);
        if (user?.RememberToken is null) return null;

        return BCryptNet.Verify(token, user.RememberToken) ? user : null;
    }

    public async Task<string?> IssueRememberTokenAsync(User user)
    {
        if (user == null) throw new ArgumentNullException(nameof(user));

        var persistedUser = await FindUserAsync(user.Username);
        if (persistedUser == null) return null;

        byte[] tokenBytes = RandomNumberGenerator.GetBytes(32);
        var token = Convert.ToBase64String(tokenBytes);
        persistedUser.RememberToken = BCryptNet.HashPassword(token);
        _ = await _databaseService.UpdateItemAsync(persistedUser);
        return token;
    }

    public async Task ClearRememberTokenAsync(User user)
    {
        if (user == null) throw new ArgumentNullException(nameof(user));

        var persistedUser = await FindUserAsync(user.Username);
        if (persistedUser?.RememberToken == null) return;

        persistedUser.RememberToken = null;
        _ = await _databaseService.UpdateItemAsync(persistedUser);
    }

    public async Task<bool> RegisterAsync(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password)) return false;

        var normalizedUsername = username.Trim();
        var normalizedPassword = password.Trim();

        var existingUser = await FindUserAsync(normalizedUsername);
        if (existingUser != null)
            return false;

        User newUser = new()
        {
            Username = normalizedUsername,
            Password = BCryptNet.HashPassword(normalizedPassword),
            RememberToken = null
        };

        _ = await _databaseService.AddItemAsync(newUser);
        return true;
    }

    public async Task LogoutAsync()
    {
        await Task.CompletedTask;
    }

    private async Task<User?> FindUserAsync(string username)
    {
        if (string.IsNullOrWhiteSpace(username)) return null;
        var normalizedUsername = username.Trim();

        IEnumerable<User> users = await _databaseService.GetItemsAsync();

        return users.FirstOrDefault(u =>
            string.Equals(u.Username, normalizedUsername, StringComparison.OrdinalIgnoreCase));
    }
}
