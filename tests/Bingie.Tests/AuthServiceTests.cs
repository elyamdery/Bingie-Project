using System.Linq;
using Bingie.Models;
using Bingie.Services;
using Bingie.Tests.TestDoubles;
using BCryptNet = BCrypt.Net.BCrypt;

namespace Bingie.Tests;

public class AuthServiceTests
{
    [Fact]
    public async Task LoginAsync_TrimsCredentialsBeforeVerification()
    {
        // Arrange
        var store = new InMemoryUserStore(new[]
        {
            new User
            {
                Id = 1,
                Username = "tester",
                Password = BCryptNet.HashPassword("secret"),
                RememberToken = null
            }
        });

        var authService = new AuthService(store);

        // Act
        var user = await authService.LoginAsync(" tester ", " secret ");

        // Assert
        Assert.NotNull(user);
        Assert.Equal(1, user!.Id);
    }

    [Fact]
    public async Task IssueRememberTokenAsync_PersistsHashedTokenAndReturnsPlaintext()
    {
        // Arrange
        var user = new User
        {
            Id = 1,
            Username = "remember-me",
            Password = BCryptNet.HashPassword("safe"),
            RememberToken = null
        };

        var store = new InMemoryUserStore(new[] { user });
        var authService = new AuthService(store);

        // Act
        var token = await authService.IssueRememberTokenAsync(user);

        // Assert
        Assert.False(string.IsNullOrWhiteSpace(token));

        var storedUser = await store.GetItemAsync("1");
        Assert.NotNull(storedUser);
        Assert.NotNull(storedUser!.RememberToken);
        Assert.True(BCryptNet.Verify(token!, storedUser.RememberToken));

        var tokenLogin = await authService.LoginWithTokenAsync(user.Username, token!);
        Assert.NotNull(tokenLogin);
        Assert.Equal(user.Username, tokenLogin!.Username);
    }

    [Fact]
    public async Task ClearRememberTokenAsync_RemovesPersistedToken()
    {
        // Arrange
        var user = new User
        {
            Id = 2,
            Username = "cleanup",
            Password = BCryptNet.HashPassword("secure"),
            RememberToken = BCryptNet.HashPassword("token")
        };

        var store = new InMemoryUserStore(new[] { user });
        var authService = new AuthService(store);

        // Act
        await authService.ClearRememberTokenAsync(user);

        // Assert
        var storedUser = await store.GetItemAsync("2");
        Assert.NotNull(storedUser);
        Assert.Null(storedUser!.RememberToken);
    }

    [Fact]
    public async Task LoginWithTokenAsync_ReturnsNullForInvalidToken()
    {
        // Arrange
        var user = new User
        {
            Id = 3,
            Username = "token-user",
            Password = BCryptNet.HashPassword("pw"),
            RememberToken = BCryptNet.HashPassword("valid-token")
        };

        var store = new InMemoryUserStore(new[] { user });
        var authService = new AuthService(store);

        // Act
        var result = await authService.LoginWithTokenAsync(user.Username, "wrong-token");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task RegisterAsync_PersistsTrimmedCredentialsAndAllowsLogin()
    {
        var store = new InMemoryUserStore();
        var authService = new AuthService(store);

        var registered = await authService.RegisterAsync("  CalmUser  ", " breathe ");
        Assert.True(registered);

        var persisted = (await store.GetItemsAsync()).Single();
        Assert.Equal("CalmUser", persisted.Username);
        Assert.True(BCryptNet.Verify("breathe", persisted.Password));

        var login = await authService.LoginAsync("calmuser", "breathe");
        Assert.NotNull(login);
        Assert.Equal(persisted.Username, login!.Username);
    }

    [Fact]
    public async Task RegisterAsync_PreventsDuplicateUsernamesIgnoringCase()
    {
        var existing = new User
        {
            Id = 42,
            Username = "Gentle",
            Password = BCryptNet.HashPassword("pw")
        };

        var store = new InMemoryUserStore(new[] { existing });
        var authService = new AuthService(store);

        var result = await authService.RegisterAsync("gentle", "other");
        Assert.False(result);
    }

    [Fact]
    public async Task LoginAsync_ReturnsNullForUnknownUser()
    {
        var store = new InMemoryUserStore();
        var authService = new AuthService(store);

        var result = await authService.LoginAsync("missing", "pw");
        Assert.Null(result);
    }

    [Fact]
    public async Task LoginAsync_ReturnsNullWhenPasswordDoesNotMatch()
    {
        var store = new InMemoryUserStore(new[]
        {
            new User { Id = 3, Username = "Mismatch", Password = BCryptNet.HashPassword("aligned") }
        });

        var authService = new AuthService(store);

        var result = await authService.LoginAsync("Mismatch", "different");
        Assert.Null(result);
    }
}
