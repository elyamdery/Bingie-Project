using Bingie.Models;
using Bingie.Services;

namespace Bingie.Views;

public partial class BingeRecordsPage : ContentPage
{
    private readonly IDataStore<BingeEntry> _dataStore;
    private readonly string _username;

    public BingeRecordsPage(IDataStore<BingeEntry> dataStore, string username)
    {
        InitializeComponent();
        _dataStore = dataStore ?? throw new ArgumentNullException(nameof(dataStore));
        _username = username ?? throw new ArgumentNullException(nameof(username));
        LoadBingeRecordsAsync();
    }

    private async void LoadBingeRecordsAsync()
    {
        try
        {
            IEnumerable<BingeEntry> records = await _dataStore.GetItemsAsync();
            List<BingeEntry> todayRecords =
                records.Where(r => r.Date.Date == DateTime.Today && r.Username == _username).ToList();
            BingeRecordsCollectionView.ItemsSource = todayRecords;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to load records: {ex.Message}", "OK");
        }
    }

    private async void OnAddBingeRecordClicked(object sender, EventArgs e)
    {
        try
        {
            BingeEntry newRecord = new()
            {
                Username = _username,
                Date = DateTime.Now,
                Duration = TimeSpan.FromMinutes(20) // Example duration
            };

            var success = await _dataStore.AddItemAsync(newRecord);

            if (success)
            {
                await DisplayAlert("Success", "Binge record added successfully!", "OK");
                LoadBingeRecordsAsync(); // Refresh records
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to add record: {ex.Message}", "OK");
        }
    }
}