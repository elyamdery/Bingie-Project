using System.Security.Cryptography;
using System.Text;

namespace Bingie.Services;

public class TokenService
{
    private const string TokenKey = "AuthToken";
    private const string UserKey = "CurrentUser";
    private const int TokenExpirationHours = 24;

    public string GenerateToken(string username)
    {
        // Create a unique token with username and expiration time
        var expiration = DateTime.UtcNow.AddHours(TokenExpirationHours);
        var tokenData = $"{username}|{expiration:O}|{GenerateRandomString(16)}";
        
        // Store the token in secure storage
        SecureStorage.SetAsync(TokenKey, tokenData).Wait();
        Preferences.Set(UserKey, username);
        
        return tokenData;
    }

    public bool ValidateToken()
    {
        try
        {
            var tokenData = SecureStorage.GetAsync(TokenKey).Result;
            if (string.IsNullOrEmpty(tokenData))
                return false;

            var parts = tokenData.Split('|');
            if (parts.Length != 3)
                return false;

            var expiration = DateTime.Parse(parts[1]);
            return expiration > DateTime.UtcNow;
        }
        catch
        {
            return false;
        }
    }

    public string GetCurrentUsername()
    {
        return Preferences.Get(UserKey, string.Empty);
    }

    public void ClearToken()
    {
        SecureStorage.Remove(TokenKey);
        Preferences.Remove(UserKey);
    }

    private string GenerateRandomString(int length)
    {
        var bytes = RandomNumberGenerator.GetBytes(length);
        return Convert.ToBase64String(bytes);
    }
}
