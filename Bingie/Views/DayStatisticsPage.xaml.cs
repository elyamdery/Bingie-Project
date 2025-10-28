using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bingie.Models;
using Bingie.Services;

namespace Bingie.Views;

public partial class DayStatisticsPage : ContentPage
{
    private readonly DateTime _selectedDate;
    private readonly string _username;
    private readonly CalendarService _calendarService;

    public DayStatisticsPage()
    {
        InitializeComponent();
        var connectionFactory = new SqliteConnectionFactory();
        var previewService = new DatabaseService(connectionFactory);
        _calendarService = new CalendarService((IDataStore<BingeEntry>)previewService);
        _username = "PreviewUser";
        _selectedDate = DateTime.Today;
        _ = DisplayStatisticsAsync();
    }

    public DayStatisticsPage(DateTime date, CalendarService calendarService, string username)
    {
        InitializeComponent();
        _selectedDate = date;
        _calendarService = calendarService ?? throw new ArgumentNullException(nameof(calendarService));
        _username = username ?? throw new ArgumentNullException(nameof(username));
        _ = DisplayStatisticsAsync();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        MessagingCenter.Subscribe<MainPage, BingeEntry>(this, "BingeEntryAdded",
            async (_, entry) =>
            {
                if (!string.Equals(entry.Username, _username, StringComparison.OrdinalIgnoreCase))
                    return;

                if (entry.Date.ToLocalTime().Date != _selectedDate.Date)
                    return;

                await DisplayStatisticsAsync();
            });
    }

    protected override void OnDisappearing()
    {
        MessagingCenter.Unsubscribe<MainPage, BingeEntry>(this, "BingeEntryAdded");
        base.OnDisappearing();
    }

    private async Task DisplayStatisticsAsync()
    {
        try
        {
            SelectedDateLabel.Text = _selectedDate.ToString("MMMM dd, yyyy");
            IReadOnlyList<BingeEntry> entries =
                await _calendarService.GetEntriesForDateAsync(_username, _selectedDate);

            var bingeCount = entries.Count;
            var suffix = bingeCount == 1 ? string.Empty : "s";

            if (bingeCount == 0)
            {
                BingeCountLabel.Text = "No binges logged—great work!";
            }
            else
            {
                BingeCountLabel.Text = $"You logged {bingeCount} time{suffix} today.";
            }

            var friendlyEntries = entries
                .OrderBy(entry => entry.Date)
                .Select(entry => new DayEntryViewModel(entry))
                .ToList();

            EntriesCollectionView.ItemsSource = friendlyEntries;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in DisplayStatistics: {ex.Message}");
            BingeCountLabel.Text = "Unable to retrieve binge count.";
        }
    }

    private void OnBackButtonClicked(object sender, EventArgs e)
    {
        _ = Navigation.PopAsync();
    }

    private sealed record DayEntryViewModel
    {
        public DayEntryViewModel(BingeEntry entry)
        {
            var localTime = entry.Date.ToLocalTime();
            DisplayTime = localTime.ToString("HH:mm");
            FriendlyNote = entry.Duration > TimeSpan.Zero
                ? $"Reflection logged (~{Math.Max(1, (int)Math.Round(entry.Duration.TotalMinutes))} min)"
                : "Quick check-in captured.";
        }

        public string DisplayTime { get; }
        public string FriendlyNote { get; }
    }
}
