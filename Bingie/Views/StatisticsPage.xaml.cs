using Bingie.Models;
using Bingie.Services;

namespace Bingie.Views;

public partial class StatisticsPage : ContentPage
{
    private readonly IDataStore<BingeEntry> _dataStore;
    private readonly string _username;

    public StatisticsPage(IDataStore<BingeEntry> dataStore, string username)
    {
        InitializeComponent();
        _dataStore = dataStore ?? throw new ArgumentNullException(nameof(dataStore));
        _username = username ?? throw new ArgumentNullException(nameof(username));

        // Load and display the binge count for the user
        LoadBingeCountAsync();

        // Start the page animation (fade in)
        StartPageAnimation();
    }

    private async void LoadBingeCountAsync()
    {
        try
        {
            IEnumerable<BingeEntry> records = await _dataStore.GetItemsAsync();
            var bingeCount = records.Count(r => r.Username == _username);
            BingeCountLabel.Text = $"Binge Count: {bingeCount}";
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to load binge count: {ex.Message}", "OK");
        }
    }

    private async void StartPageAnimation()
    {
        // Fade in animation
        Opacity = 0;
        _ = await this.FadeTo(1, 500); // Fades in over 0.5 seconds
    }
}