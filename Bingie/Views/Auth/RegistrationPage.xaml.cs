using System;
using Bingie.Services;

namespace Bingie.Views.Auth;

public partial class RegistrationPage : ContentPage
{
    private readonly IAuthService _authService;

    public RegistrationPage(IAuthService authService)
    {
        InitializeComponent();
        _authService = authService ?? throw new ArgumentNullException(nameof(authService));
    }

    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        ErrorLabel.IsVisible = false;
        ErrorLabel.Text = string.Empty;

        var username = UsernameEntry.Text?.Trim() ?? string.Empty;
        var password = PasswordEntry.Text ?? string.Empty;
        var confirm = ConfirmPasswordEntry.Text ?? string.Empty;

        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            ShowError("Please choose a username and password.");
            return;
        }

        if (!string.Equals(password, confirm, StringComparison.Ordinal))
        {
            ShowError("Passwords need to match.");
            return;
        }

        var submitButton = sender as Button;
        if (submitButton != null) submitButton.IsEnabled = false;

        try
        {
            var registrationSuccess = await _authService.RegisterAsync(username, password);

            if (!registrationSuccess)
            {
                ShowError("That username is already taken. Try another one.");
                return;
            }

            await DisplayAlert("Welcome", "Account created! Sign in with your new details.", "Nice");
            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            ShowError("We couldn't finish registration. Please try again." );
            System.Diagnostics.Debug.WriteLine(ex);
        }
        finally
        {
            if (submitButton != null) submitButton.IsEnabled = true;
        }
    }

    private void ShowError(string message)
    {
        ErrorLabel.Text = message;
        ErrorLabel.IsVisible = true;
    }
}
