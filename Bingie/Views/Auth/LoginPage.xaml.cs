using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Bingie.Models;
using Bingie.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Bingie.Views.Auth;

public partial class LoginPage : ContentPage
{
    private const string RememberedUsernameKey = "RememberedUsername";
    private const string RememberTokenKey = "RememberedToken";
    private const string RememberMeFlagKey = "RememberMeEnabled";

    private readonly IAuthService _authService;
    private readonly IDataStore<BingeEntry> _bingeEntryStore;
    private readonly CalendarService _calendarService;
    private readonly AvatarFeedbackService _avatarFeedbackService;
    private readonly StoryGuideService _storyGuideService;
    private readonly PointsSystemService _pointsSystemService;
    private readonly FriendsLeaderboardService _friendsLeaderboardService;
    private readonly IServiceProvider _serviceProvider;

    private bool _isAuthenticating;

    public LoginPage(
        IAuthService authService,
        IDataStore<BingeEntry> bingeEntryStore,
        CalendarService calendarService,
        AvatarFeedbackService avatarFeedbackService,
        StoryGuideService storyGuideService,
        PointsSystemService pointsSystemService,
        FriendsLeaderboardService friendsLeaderboardService,
        IServiceProvider serviceProvider)
    {
        InitializeComponent();
        _authService = authService ?? throw new ArgumentNullException(nameof(authService));
        _bingeEntryStore = bingeEntryStore ?? throw new ArgumentNullException(nameof(bingeEntryStore));
        _calendarService = calendarService ?? throw new ArgumentNullException(nameof(calendarService));
        _avatarFeedbackService = avatarFeedbackService ?? throw new ArgumentNullException(nameof(avatarFeedbackService));
        _storyGuideService = storyGuideService ?? throw new ArgumentNullException(nameof(storyGuideService));
        _pointsSystemService = pointsSystemService ?? throw new ArgumentNullException(nameof(pointsSystemService));
        _friendsLeaderboardService = friendsLeaderboardService ?? throw new ArgumentNullException(nameof(friendsLeaderboardService));
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await RestoreSessionAsync();
    }

    private async Task RestoreSessionAsync()
    {
        try
        {
            var rememberedUsername = Preferences.Get(RememberedUsernameKey, string.Empty);
            if (!string.IsNullOrEmpty(rememberedUsername))
            {
                UsernameEntry.Text = rememberedUsername;
            }

            var rememberMeEnabled = Preferences.Get(RememberMeFlagKey, false);
            RememberMeCheckBox.IsChecked = rememberMeEnabled;
            SecureStorage.Remove("RememberedPassword"); // Legacy cleanup

            if (!rememberMeEnabled || string.IsNullOrEmpty(rememberedUsername)) return;

            var token = await SecureStorage.GetAsync(RememberTokenKey);
            if (string.IsNullOrEmpty(token))
            {
                RememberMeCheckBox.IsChecked = false;
                Preferences.Set(RememberMeFlagKey, false);
                return;
            }

            var user = await _authService.LoginWithTokenAsync(rememberedUsername, token);
            if (user != null)
            {
                await NavigateToShellAsync(user, displaySuccessMessage: false);
                return;
            }

            SecureStorage.Remove(RememberTokenKey);
            RememberMeCheckBox.IsChecked = false;
            Preferences.Set(RememberMeFlagKey, false);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Session restoration failed: {ex.Message}");
            RememberMeCheckBox.IsChecked = false;
        }
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        if (_isAuthenticating) return;

        var username = UsernameEntry.Text?.Trim() ?? string.Empty;
        var password = PasswordEntry.Text ?? string.Empty;

        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            await DisplayAlert("Error", "Username and password cannot be empty.", "OK");
            return;
        }

        _isAuthenticating = true;
        LoginButton.IsEnabled = false;

        try
        {
            var user = await _authService.LoginAsync(username, password);
            if (user == null)
            {
                await DisplayAlert("Error", "Invalid username or password.", "OK");
                return;
            }

            await HandleRememberMeAsync(user);
            await NavigateToShellAsync(user, displaySuccessMessage: true);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Login failed: {ex.Message}");
            await DisplayAlert("Error", "Something went wrong during login. Please try again.", "OK");
        }
        finally
        {
            _isAuthenticating = false;
            LoginButton.IsEnabled = true;
        }
    }

    private async Task HandleRememberMeAsync(User user)
    {
        if (RememberMeCheckBox.IsChecked)
        {
            Preferences.Set(RememberedUsernameKey, user.Username);
            Preferences.Set(RememberMeFlagKey, true);

            try
            {
                var token = await _authService.IssueRememberTokenAsync(user);
                if (!string.IsNullOrEmpty(token))
                {
                    await SecureStorage.SetAsync(RememberTokenKey, token);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to persist remember-me token: {ex.Message}");
            }
        }
        else
        {
            Preferences.Remove(RememberedUsernameKey);
            Preferences.Set(RememberMeFlagKey, false);
            SecureStorage.Remove(RememberTokenKey);

            try
            {
                await _authService.ClearRememberTokenAsync(user);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to clear remember-me token: {ex.Message}");
            }
        }
    }

    private async Task NavigateToShellAsync(User user, bool displaySuccessMessage)
    {
        if (Application.Current == null)
        {
            await DisplayAlert("Error", "Unable to load the application shell.", "OK");
            return;
        }

        if (displaySuccessMessage)
        {
            await DisplayAlert("Welcome back", "Login successful!", "OK");
        }

        Application.Current.MainPage = new AppShell(
            _bingeEntryStore,
            _calendarService,
            _avatarFeedbackService,
            _storyGuideService,
            _pointsSystemService,
            _friendsLeaderboardService,
            _authService,
            user.Username);
    }

    private void OnRegisterClicked(object sender, EventArgs e)
    {
        var registrationPage = _serviceProvider.GetRequiredService<RegistrationPage>();
        _ = Navigation.PushAsync(registrationPage);
    }
}
