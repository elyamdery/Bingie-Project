using Bingie.Models;
using Bingie.Services;

namespace Bingie.Views.Auth;

public partial class LoginPage : ContentPage
{
    private readonly IAuthService _authService;

    public LoginPage(IAuthService authService)
    {
        InitializeComponent();
        _authService = authService;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Load saved username from Preferences
        var rememberedUsername = Preferences.Get("RememberedUsername", string.Empty);
        if (!string.IsNullOrEmpty(rememberedUsername))
        {
            UsernameEntry.Text = rememberedUsername;

            // Load saved password from SecureStorage
            var rememberedPassword = await SecureStorage.GetAsync("RememberedPassword");
            if (!string.IsNullOrEmpty(rememberedPassword))
            {
                PasswordEntry.Text = rememberedPassword;
                RememberMeCheckBox.IsChecked = true;
            }
        }
    }    private async void OnLoginClicked(object sender, EventArgs e)
    {
        try
        {
            var username = UsernameEntry.Text?.Trim();
            var password = PasswordEntry.Text;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                await DisplayAlert("Error", "Please enter both username and password.", "OK");
                return;
            }

            // Show loading indicator
            LoginButton.IsEnabled = false;
            LoginButton.Text = "Logging in...";

            var user = await _authService.LoginAsync(username, password);

            if (user != null)
            {
                // Handle Remember Me functionality
                if (RememberMeCheckBox.IsChecked)
                {
                    Preferences.Set("RememberedUsername", username);
                    await SecureStorage.SetAsync("RememberedPassword", password);
                }
                else
                {
                    Preferences.Remove("RememberedUsername");
                    SecureStorage.Remove("RememberedPassword");
                }                // Store current user info
                Preferences.Set("CurrentUserId", user.Id);
                Preferences.Set("CurrentUsername", user.Username);                await DisplayAlert("Success", $"Welcome back, {user.Username}!", "OK");

                // Navigate to main app with user context
                if (Application.Current != null)
                {
                    Application.Current.MainPage = new AppShell(user);
                }
            }
        }
        catch (UnauthorizedAccessException)
        {
            await DisplayAlert("Error", "Invalid username or password.", "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Login failed: {ex.Message}", "OK");
        }
        finally
        {
            // Reset button state
            LoginButton.IsEnabled = true;
            LoginButton.Text = "Login";
        }
    }    private void OnRegisterClicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new RegistrationPage(_authService));
    }
}