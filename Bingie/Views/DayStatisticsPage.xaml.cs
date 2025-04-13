using System.Diagnostics;
using Bingie.Models;
using Bingie.Services;

namespace Bingie.Views;

public partial class DayStatisticsPage : ContentPage
{
    private readonly IDataStore<BingeEntry> _dataStore;
    private readonly DateTime _selectedDate;
    private readonly string _username;

    public DayStatisticsPage(DateTime date, IDataStore<BingeEntry> dataStore, string username)
    {
        InitializeComponent();
        _selectedDate = date;
        _dataStore = dataStore ?? throw new ArgumentNullException(nameof(dataStore));
        _username = username ?? throw new ArgumentNullException(nameof(username));
        DisplayStatisticsAsync();
    }

    private async void DisplayStatisticsAsync()
    {
        try
        {
            Debug.WriteLine($"DayStatisticsPage: Displaying stats for {_selectedDate}");
            SelectedDateLabel.Text = _selectedDate.ToString("MMMM dd, yyyy");
            var bingeCount = await GetBingeCountForDateAsync(_selectedDate);
            BingeCountLabel.Text = $"You binged {bingeCount} times on this day.";
        }
        catch (Exception ex)
        {
            // Log the exception
            Console.WriteLine($"Error in DisplayStatistics: {ex.Message}");
            BingeCountLabel.Text = "Unable to retrieve binge count.";
        }
    }

    private async Task<int> GetBingeCountForDateAsync(DateTime date)
    {
        try
        {
            Debug.WriteLine($"DayStatisticsPage: Retrieving binge count for {date}");
            IEnumerable<BingeEntry> records = await _dataStore.GetItemsAsync();
            return records.Count(r => r.Username == _username && r.Date.Date == date.Date);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Exception in GetBingeCountForDateAsync: {ex.Message}");
            throw;
        }
    }

    private void OnBackButtonClicked(object sender, EventArgs e)
    {
        _ = Navigation.PopAsync();
    }
}