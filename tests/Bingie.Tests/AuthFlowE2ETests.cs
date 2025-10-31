using System.Threading.Tasks;
using Bingie.Models;
using Bingie.Services;
using Bingie.Tests.TestDoubles;
using BCryptNet = BCrypt.Net.BCrypt;

namespace Bingie.Tests;

public class AuthFlowE2ETests
{
    [Fact]
    public async Task RegisterLoginAndRememberMe_EndToEndFlow_Succeeds()
    {
        var store = new InMemoryUserStore();
        var authService = new AuthService(store);

        var registered = await authService.RegisterAsync(" mindful  ", " breathe ");
        Assert.True(registered);

        var login = await authService.LoginAsync("MINDFUL", "breathe");
        Assert.NotNull(login);

        var token = await authService.IssueRememberTokenAsync(login!);
        Assert.False(string.IsNullOrWhiteSpace(token));

        var autoLogin = await authService.LoginWithTokenAsync(login!.Username, token!);
        Assert.NotNull(autoLogin);

        await authService.ClearRememberTokenAsync(login);

        var afterClear = await authService.LoginWithTokenAsync(login.Username, token!);
        Assert.Null(afterClear);
    }

    [Fact]
    public async Task RegisterDuplicateUsername_EndToEndFlow_FailsGracefully()
    {
        var store = new InMemoryUserStore(new[]
        {
            new User { Id = 5, Username = "steady", Password = BCryptNet.HashPassword("breath") }
        });
        var authService = new AuthService(store);

        var result = await authService.RegisterAsync("steady", "fresh");
        Assert.False(result);

        var login = await authService.LoginAsync("steady", "breath");
        Assert.NotNull(login);
    }
}
