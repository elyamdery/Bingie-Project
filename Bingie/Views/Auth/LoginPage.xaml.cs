using Bingie.Models;
using Bingie.Services;

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

            // Create an in-memory implementation of IDataStore<BingeEntry> and pass it along with the username
            IDataStore<BingeEntry> dataStore = new InMemoryBingeEntryDataStore();
            Application.Current.MainPage = new AppShell(dataStore, username);
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