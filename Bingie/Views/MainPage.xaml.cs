using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Bingie.Models;
using Bingie.Services;
using Serilog;

namespace Bingie.Views;

public partial class MainPage : ContentPage
{
    private readonly IDataStore<BingeEntry> _dataStore;
    private readonly CalendarService _calendarService;
    private readonly string _username;

    // Default constructor for XAML preview support
    public MainPage()
    {
        InitializeComponent();
        var connectionFactory = new SqliteConnectionFactory();
        var previewService = new DatabaseService(connectionFactory);
        _dataStore = (IDataStore<BingeEntry>)previewService;
        _calendarService = new CalendarService(_dataStore);
        _username = "PreviewUser";

        StartClock();
        _ = RefreshTodayDotsAsync();
    }

    public MainPage(IDataStore<BingeEntry> dataStore, CalendarService calendarService, string username)
    {
        InitializeComponent();
        _dataStore = dataStore ?? throw new ArgumentNullException(nameof(dataStore));
        _calendarService = calendarService ?? throw new ArgumentNullException(nameof(calendarService));
        _username = username ?? throw new ArgumentNullException(nameof(username));

        // Initialize logging in the MainPage as well
        Log.Information("MainPage initialized."); // <-- Log when the MainPage is initialized

        StartClock();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await RefreshTodayDotsAsync();
    }

    private void StartClock()
    {
        Dispatcher.StartTimer(TimeSpan.FromSeconds(1), () =>
        {
            var now = DateTime.UtcNow.AddHours(3);
            var timestamp = now.ToString("HH:mm:ss", CultureInfo.InvariantCulture);
            Log.Information("Current time: {Time}", timestamp);

            CurrentTimeLabel.Text = timestamp; // Adjust to Israel Time (UTC+3)
            return true;
        });
    }

    private async Task RefreshTodayDotsAsync()
    {
        try
        {
            IEnumerable<BingeEntry> entries =
                await _calendarService.GetEntriesForDateAsync(_username, DateTime.Today);

            var entryCount = entries.Count();

            DotsContainer.Children.Clear();

            if (entryCount == 0) return;

            var rowsNeeded = (int)Math.Ceiling(entryCount / 10.0);
            var entryIndex = 0;

            for (var row = 0; row < rowsNeeded; row++)
            {
                StackLayout currentRow = new()
                {
                    Orientation = StackOrientation.Horizontal,
                    Margin = new Thickness(0, 5)
                };

                for (var col = 0; col < 10 && entryIndex < entryCount; col++, entryIndex++)
                {
                    currentRow.Children.Add(CreateBingeDot());
                }

                DotsContainer.Children.Add(currentRow);
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to refresh today's binge dots.");
        }
    }

    private static BoxView CreateBingeDot()
    {
        return new BoxView
        {
            Color = Colors.Red,
            WidthRequest = 15,
            HeightRequest = 15,
            Margin = new Thickness(2, 0, 2, 0)
        };
    }

    private async void OnBingeButtonClicked(object sender, EventArgs e)
    {
        // Log when the binge button is clicked
        Log.Information("Binge button clicked at {Time}", DateTime.UtcNow.ToString("HH:mm:ss"));

        try
        {
            BingeButton.IsEnabled = false;
            BingeEntry newEntry = new()
            {
                Username = _username,
                Date = DateTime.UtcNow,
                Duration = TimeSpan.Zero
            };

            var saved = await _dataStore.AddItemAsync(newEntry);
            if (!saved)
            {
                await ShowFeedbackAsync("We couldn't save that moment. Please try again.", isError: true);
                return;
            }

            Log.Information("Binge entry saved for {Username} at {Timestamp}", _username, newEntry.Date);

            await RefreshTodayDotsAsync();

            MessagingCenter.Send(this, "BingeEntryAdded", newEntry);

            await ShowFeedbackAsync("Logged! Thanks for checking in 💪");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to record binge entry.");
            await ShowFeedbackAsync("Couldn't save that. Try again?", isError: true);
        }
        finally
        {
            BingeButton.IsEnabled = true;
        }
    }

    private async Task ShowFeedbackAsync(string message, bool isError = false)
    {
        FeedbackLabel.Text = message;
        FeedbackLabel.TextColor = isError ? Colors.Yellow : Colors.LawnGreen;
        _ = FeedbackLabel.FadeTo(1, 150);
        await Task.Delay(TimeSpan.FromSeconds(2.2));
        await FeedbackLabel.FadeTo(0, 400);
    }
}
