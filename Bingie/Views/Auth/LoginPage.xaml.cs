using Bingie.Models;
using Bingie.Services;
using System.Diagnostics;

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
        try
        {
            // TEMPORARY: Bypass login completely for testing
            // Create a database-backed implementation of IDataStore<BingeEntry>
            SqliteConnectionFactory factory = new();
            IDataStore<BingeEntry> store = new DatabaseService(factory);
            Application.Current.MainPage = new AppShell(store, "test");
            return;

            // Show loading indicator
            ActivityIndicator.IsVisible = true;
            LoginButton.IsEnabled = false;
            RegisterButton.IsEnabled = false;

            var username = UsernameEntry.Text;
            var password = PasswordEntry.Text;

            // Validate input
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                await DisplayAlert("Error", "Please enter both username and password.", "OK");
                return;
            }

            // For testing purposes, allow login with test/test
            if (username == "test" && password == "test")
            {
                await DisplayAlert("Success", "Login successful with test account!", "OK");

                // Create a database-backed implementation of IDataStore<BingeEntry>
                SqliteConnectionFactory cf = new();
                IDataStore<BingeEntry> ds = new DatabaseService(cf);
                Application.Current.MainPage = new AppShell(ds, username);
                return;
            }

            Debug.WriteLine($"Attempting to login with username: {username}");
            var isLoggedIn = await _authService.LoginAsync(username, password);
            Debug.WriteLine($"Login result: {isLoggedIn}");

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

                // Create a database-backed implementation of IDataStore<BingeEntry>
                SqliteConnectionFactory connectionFactory = new();
                IDataStore<BingeEntry> dataStore = new DatabaseService(connectionFactory);
                Application.Current.MainPage = new AppShell(dataStore, username);
            }
            else
            {
                await DisplayAlert("Error", "Invalid username or password. Try using test/test.", "OK");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Login error: {ex.Message}");
            Debug.WriteLine($"Stack trace: {ex.StackTrace}");
            await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
        }
        finally
        {
            // Hide loading indicator
            ActivityIndicator.IsVisible = false;
            LoginButton.IsEnabled = true;
            RegisterButton.IsEnabled = true;
        }
    }

    private void OnRegisterClicked(object sender, EventArgs e)
    {
        _ = Navigation.PushAsync(new RegistrationPage(_authService));
    }

    // In-memory implementation of IDataStore<BingeEntry>
    private class InMemoryBingeEntryDataStore : IDataStore<BingeEntry>
    {
        private readonly List<BingeEntry> _items = new();

        public Task<bool> AddItemAsync(BingeEntry item)
        {
            _items.Add(item);
            return Task.FromResult(true);
        }

        public Task<bool> UpdateItemAsync(BingeEntry item)
        {
            var oldItem = _items.Find(x => x.Id == item.Id);
            if (oldItem != null)
            {
                _items.Remove(oldItem);
                _items.Add(item);
                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }

        public Task<bool> DeleteItemAsync(string id)
        {
            var oldItem = _items.Find(x => x.Id.ToString() == id);
            if (oldItem != null)
            {
                _items.Remove(oldItem);
                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }

        public Task<BingeEntry> GetItemAsync(string id)
        {
            return Task.FromResult(_items.Find(x => x.Id.ToString() == id));
        }

        public Task<IEnumerable<BingeEntry>> GetItemsAsync()
        {
            return Task.FromResult<IEnumerable<BingeEntry>>(_items);
        }
    }
}