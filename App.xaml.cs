using Bingie.Data;
using Bingie.Services;
using Bingie.Views.Auth;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Bingie;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        Log.Information("Application Started");

        // Use dependency injection to get the AuthService
        var authService = IPlatformApplication.Current?.Services?.GetService<IAuthService>();
        if (authService == null)
        {
            Log.Error("Failed to resolve AuthService from DI container");
            // Fallback: Create services manually if DI fails
            var context = IPlatformApplication.Current?.Services?.GetService<AppDBContext>();
            if (context != null)
            {
                authService = new AuthService(context);
            }
        }

        if (authService != null)
        {
            MainPage = new NavigationPage(new LoginPage(authService));
        }
        else
        {
            Log.Error("Failed to create AuthService");
            MainPage = new ContentPage 
            { 
                Content = new Label 
                { 
                    Text = "Application failed to initialize", 
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center
                }
            };
        }
    }
}