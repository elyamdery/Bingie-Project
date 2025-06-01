using Bingie.Services;

namespace Bingie.Views.Auth;

public partial class RegistrationPage : ContentPage
{
    private readonly IAuthService _authService;

    public RegistrationPage(IAuthService authService)
    {
        InitializeComponent();
        _authService = authService;
    }

    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        try
        {
            var username = UsernameEntry.Text?.Trim();
            var email = EmailEntry.Text?.Trim();
            var password = PasswordEntry.Text;
            var confirmPassword = ConfirmPasswordEntry.Text;

            // Validate input
            if (string.IsNullOrWhiteSpace(username))
            {
                await DisplayAlert("Error", "Username is required.", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(email))
            {
                await DisplayAlert("Error", "Email is required.", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                await DisplayAlert("Error", "Password is required.", "OK");
                return;
            }

            if (password != confirmPassword)
            {
                await DisplayAlert("Error", "Passwords do not match.", "OK");
                return;
            }

            // Show loading indicator
            RegisterButton.IsEnabled = false;
            RegisterButton.Text = "Creating Account...";

            var user = await _authService.RegisterAsync(username, password, email);

            if (user != null)
            {
                await DisplayAlert("Success", $"Welcome to Bingie, {user.Username}! Your account has been created successfully.", "OK");
                await Navigation.PopAsync();
            }
        }
        catch (ArgumentException ex)
        {
            await DisplayAlert("Validation Error", ex.Message, "OK");
        }
        catch (InvalidOperationException ex)
        {
            await DisplayAlert("Registration Error", ex.Message, "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Registration failed: {ex.Message}", "OK");
        }
        finally
        {
            // Reset button state
            RegisterButton.IsEnabled = true;
            RegisterButton.Text = "Register";
        }
    }
}