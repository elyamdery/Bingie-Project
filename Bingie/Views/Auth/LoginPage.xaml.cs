namespace Bingie.Views.Auth;

public partial class LoginPage : ContentPage
{
    private readonly AuthService _authService;

    public LoginPage(AuthService authService)
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
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        var username = UsernameEntry.Text;
        var password = PasswordEntry.Text;

        var isLoggedIn = await _authService.LoginAsync(username, password);

        if (isLoggedIn)
        {
            if (RememberMeCheckBox.IsChecked)
            {
                Preferences.Set("RememberedUsername", username);
                await SecureStorage.SetAsync("RememberedPassword", password);
            }
            else
            {
                Preferences.Remove("RememberedUsername");
                _ = SecureStorage.Remove("RememberedPassword");
            }

            await DisplayAlert("Success", "Login successful!", "OK");
            Application.Current.MainPage = new AppShell();
        }
        else
        {
            await DisplayAlert("Error", "Invalid username or password.", "OK");
        }
    }

    private void OnRegisterClicked(object sender, EventArgs e)
    {
        _ = Navigation.PushAsync(new RegistrationPage(_authService));
    }
}