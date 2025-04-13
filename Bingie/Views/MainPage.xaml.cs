using Bingie.Models;
using Bingie.Services;
using Serilog;
using System.Collections.ObjectModel;

namespace Bingie.Views;

public partial class MainPage : ContentPage
{
    private readonly IDataStore<BingeEntry> _dataStore;
    private readonly string _username;
    private int _bingeCount;
    private readonly string[] _motivationalQuotes = new[]
    {
        "Every day is a new beginning. Take a deep breath and start again.",
        "Progress, not perfection.",
        "You are stronger than your cravings.",
        "Small steps lead to big changes.",
        "Recovery is not linear, but it is possible.",
        "Be patient with yourself. Self-growth is tender.",
        "Your worth is not measured by what you eat or don't eat.",
        "Focus on how far you've come, not how far you have to go."
    };

    // Default constructor
    public MainPage()
    {
        InitializeComponent();
        // Initialize with default values or handle accordingly
        _dataStore = null;
        _username = string.Empty;
        _bingeCount = 0;
        StartClock(); // Ensure the clock starts in the default constructor
        UpdateBingeCountLabel();
        SetRandomMotivationalQuote();
    }

    public MainPage(IDataStore<BingeEntry> dataStore, string username)
    {
        InitializeComponent();
        _dataStore = dataStore ?? throw new ArgumentNullException(nameof(dataStore));
        _username = username ?? throw new ArgumentNullException(nameof(username));
        _bingeCount = 0;

        // Initialize logging in the MainPage as well
        Log.Information("MainPage initialized for user: {Username}", username);

        StartClock();
        UpdateBingeCountLabel();
        SetRandomMotivationalQuote();
        LoadTodaysBinges();
    }

    private async void LoadTodaysBinges()
    {
        try
        {
            if (_dataStore == null) return;

            var allEntries = await _dataStore.GetItemsAsync();
            var todayEntries = allEntries.Where(e =>
                e.Username == _username &&
                e.Date.Date == DateTime.Today).ToList();

            _bingeCount = todayEntries.Count;
            UpdateBingeCountLabel();
            UpdateDotsDisplay(todayEntries.Count);

            Log.Information("Loaded {Count} binge entries for today", todayEntries.Count);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error loading today's binge entries");
            await DisplayAlert("Error", "Could not load today's entries: " + ex.Message, "OK");
        }
    }

    private void UpdateDotsDisplay(int count)
    {
        DotsContainer.Children.Clear();

        for (int i = 0; i < count; i++)
        {
            Frame dotFrame = new()
            {
                BackgroundColor = Colors.Red,
                CornerRadius = 8,
                WidthRequest = 16,
                HeightRequest = 16,
                Padding = 0,
                HasShadow = true
            };

            DotsContainer.Children.Add(dotFrame);
        }
    }

    private void SetRandomMotivationalQuote()
    {
        Random random = new();
        int index = random.Next(_motivationalQuotes.Length);
        MotivationLabel.Text = _motivationalQuotes[index];
    }

    private void StartClock()
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            // Update time immediately
            UpdateTime();

            // Then start timer
            var timer = Application.Current.Dispatcher.CreateTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += (s, e) => UpdateTime();
            timer.Start();
        });
    }

    private void UpdateTime()
    {
        // Get current time in Israel (UTC+3)
        var israelTime = DateTime.UtcNow.AddHours(3);
        CurrentTimeLabel.Text = israelTime.ToString("HH:mm:ss");

        // Log time update less frequently (once per minute) to reduce log spam
        if (israelTime.Second == 0)
        {
            Log.Information("Current time: {Time}", israelTime.ToString("HH:mm:ss"));
        }
    }

    private async void OnBingeButtonClicked(object sender, EventArgs e)
    {
        try
        {
            // Log when the binge button is clicked
            Log.Information("Binge button clicked at {Time}", DateTime.UtcNow.ToString("HH:mm:ss"));

            // Add visual indicator
            Frame dotFrame = new()
            {
                BackgroundColor = Colors.Red,
                CornerRadius = 8,
                WidthRequest = 16,
                HeightRequest = 16,
                Padding = 0,
                HasShadow = true
            };

            DotsContainer.Children.Add(dotFrame);

            // Update count
            _bingeCount++;
            UpdateBingeCountLabel();

            // Save to database if available
            if (_dataStore != null)
            {
                BingeEntry newEntry = new()
                {
                    Username = _username,
                    Date = DateTime.Now,
                    Duration = TimeSpan.FromMinutes(15) // Default duration
                };

                await _dataStore.AddItemAsync(newEntry);
                Log.Information("Added new binge entry to database");
            }

            // Visual feedback
            await this.ScaleTo(0.95, 100);
            await this.ScaleTo(1, 100);

            // Change motivational quote
            SetRandomMotivationalQuote();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error recording binge event");
            await DisplayAlert("Error", "Could not record binge: " + ex.Message, "OK");
        }
    }

    private void UpdateBingeCountLabel()
    {
        BingeCountLabel.Text = $"Today's Count: {_bingeCount}";
    }
}
