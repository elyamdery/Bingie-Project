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
        var username = UsernameEntry.Text;
        var password = PasswordEntry.Text;

        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            await DisplayAlert("Error", "Username and password cannot be empty.", "OK");
            return;
        }

        var registrationSuccess = await _authService.RegisterAsync(username, password);

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
}