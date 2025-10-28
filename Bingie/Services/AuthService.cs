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

        var normalizedUsername = username.Trim();
        var normalizedPassword = password.Trim();

        IEnumerable<User> users = await _databaseService.GetItemsAsync();

        var user = users.FirstOrDefault(u =>
            string.Equals(u.Username, normalizedUsername, StringComparison.OrdinalIgnoreCase));

        if (user == null) return null;

        return BCryptNet.Verify(normalizedPassword, user.Password) ? user : null;
    }

    public async Task<User?> LoginWithTokenAsync(string username, string token)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(token)) return null;

        var normalizedUsername = username.Trim();

        IEnumerable<User> users = await _databaseService.GetItemsAsync();

        var user = users.FirstOrDefault(u =>
            string.Equals(u.Username, normalizedUsername, StringComparison.OrdinalIgnoreCase));

        if (user?.RememberToken is null) return null;

        return BCryptNet.Verify(token, user.RememberToken) ? user : null;
    }

    public async Task<string?> IssueRememberTokenAsync(User user)
    {
        if (user == null) throw new ArgumentNullException(nameof(user));

        byte[] tokenBytes = RandomNumberGenerator.GetBytes(32);
        var token = Convert.ToBase64String(tokenBytes);
        user.RememberToken = BCryptNet.HashPassword(token);
        _ = await _databaseService.UpdateItemAsync(user);
        return token;
    }

    public async Task ClearRememberTokenAsync(User user)
    {
        if (user == null) throw new ArgumentNullException(nameof(user));

        if (user.RememberToken == null) return;

        user.RememberToken = null;
        _ = await _databaseService.UpdateItemAsync(user);
    }

    public async Task<bool> RegisterAsync(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password)) return false;

        var normalizedUsername = username.Trim();
        var normalizedPassword = password.Trim();

        IEnumerable<User> users = await _databaseService.GetItemsAsync();
        if (users.Any(u =>
                string.Equals(u.Username, normalizedUsername, StringComparison.OrdinalIgnoreCase)))
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
}

