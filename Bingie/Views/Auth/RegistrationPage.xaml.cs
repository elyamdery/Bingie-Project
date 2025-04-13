using Bingie.Services;
using System.Diagnostics;

namespace Bingie.Views.Auth;

public partial class RegistrationPage : ContentPage
{
    private readonly AuthService _authService;

    public RegistrationPage(AuthService authService)
    {
        InitializeComponent();
        _authService = authService;
    }

    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        try
        {
            var username = UsernameEntry.Text;
            var password = PasswordEntry.Text;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                await DisplayAlert("Error", "Username and password cannot be empty.", "OK");
                return;
            }

            // For testing purposes, allow a quick registration with test/test
            if (username == "test")
            {
                await DisplayAlert("Info", "The test user already exists. Try a different username or use test/test to login.", "OK");
                return;
            }

            Debug.WriteLine($"Attempting to register user: {username}");
            var registrationSuccess = await _authService.RegisterAsync(username, password);
            Debug.WriteLine($"Registration result: {registrationSuccess}");

            if (registrationSuccess)
            {
                await DisplayAlert("Success", "Registration successful!", "OK");
                _ = await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("Error", "User already exists or registration failed.", "OK");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Registration error: {ex.Message}");
            Debug.WriteLine($"Stack trace: {ex.StackTrace}");
            await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
        }
    }
}