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
    private int _totalPoints;
    private bool _isUrgeSurfing;
    private CancellationTokenSource _urgeSurfingCts;
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

            // Use the new method from DatabaseService if available
            IEnumerable<BingeEntry> todayEntries;
            if (_dataStore is DatabaseService databaseService)
            {
                todayEntries = await databaseService.GetTodaysBingeEntriesAsync(_username);
            }
            else
            {
                var allEntries = await _dataStore.GetItemsAsync();
                todayEntries = allEntries.Where(e =>
                    e.Username == _username &&
                    e.Date.Date == DateTime.Today).ToList();
            }

            _bingeCount = todayEntries.Count();
            UpdateBingeCountLabel();
            UpdateDotsDisplay(_bingeCount);

            // Calculate total points
            _totalPoints = todayEntries.Sum(e => e.Points);
            UpdatePointsLabel();

            Log.Information("Loaded {Count} binge entries for today with {Points} total points", _bingeCount, _totalPoints);
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

    // Track if we've shown the tip already in this session
    private bool _tipShown = false;
    private int _tipCounter = 0;

    private async void OnBingeButtonClicked(object sender, EventArgs e)
    {
        try
        {
            // Log when the binge button is clicked
            Log.Information("Binge button clicked at {Time}", DateTime.UtcNow.ToString("HH:mm:ss"));

            // Visual feedback for the button
            BingeButton.BackgroundColor = Color.FromArgb("#2874A6"); // Darker blue when pressed
            await BingeButton.ScaleTo(0.95, 100);

            // Add visual indicator
            Frame dotFrame = new()
            {
                BackgroundColor = Color.FromArgb("#3498DB"), // Blue dot instead of red
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
                // Calculate points - just 1 point for tracking
                int points = 1;

                BingeEntry newEntry = new()
                {
                    Username = _username,
                    Date = DateTime.Now,
                    Duration = TimeSpan.FromMinutes(15), // Default duration
                    WasUrgeResisted = false, // They clicked the binge button, so they didn't resist
                    Points = points,
                    Notes = "Recorded via binge button"
                };

                await _dataStore.AddItemAsync(newEntry);

                // Update total points
                _totalPoints += points;
                UpdatePointsLabel();

                Log.Information("Added new binge entry to database with {Points} points", points);
            }

            // Change motivational quote
            SetRandomMotivationalQuote();

            // Reset button appearance
            await Task.Delay(200);
            BingeButton.BackgroundColor = Color.FromArgb("#3498DB"); // Return to original color
            await BingeButton.ScaleTo(1.0, 100);

            // Only show the tip occasionally (every 3rd click) and not if already shown in this session
            _tipCounter++;
            if (!_tipShown && _tipCounter % 3 == 0)
            {
                _tipShown = true;

                // Suggest urge surfing for next time
                bool tryUrgeSurfing = await DisplayAlert("Tip",
                    "Next time you feel the urge to binge, try the Urge Surfing feature to help resist the urge and earn more points.",
                    "Try it next time", "Dismiss");

                if (tryUrgeSurfing)
                {
                    // Highlight the urge surfing button
                    await UrgeSurfingButton.ScaleTo(1.2, 300);
                    await UrgeSurfingButton.ScaleTo(1.0, 300);
                }
            }
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

    private void UpdatePointsLabel()
    {
        PointsLabel.Text = $"Points earned: {_totalPoints}";
    }

    private async void OnUrgeSurfingButtonClicked(object sender, EventArgs e)
    {
        if (_isUrgeSurfing)
        {
            // Cancel the current urge surfing session
            _urgeSurfingCts?.Cancel();
            return;
        }

        try
        {
            _isUrgeSurfing = true;
            UrgeSurfingButton.Text = "Cancel";
            UrgeSurfingButton.BackgroundColor = Colors.LightPink;

            // Create a new cancellation token source
            _urgeSurfingCts = new CancellationTokenSource();

            // Show initial dialog
            bool startSession = await DisplayAlert("Urge Surfing",
                "Urge surfing is a mindfulness technique that helps you ride out the urge to binge without acting on it. \n\n" +
                "We'll guide you through a 2-minute exercise. During this time, observe your urge without judging it. \n\n" +
                "Ready to start?", "Yes", "Cancel");

            if (!startSession)
            {
                ResetUrgeSurfingUI();
                return;
            }

            // Start the urge surfing session
            await StartUrgeSurfingSession(_urgeSurfingCts.Token);
        }
        catch (TaskCanceledException)
        {
            // Session was canceled, just reset the UI
            ResetUrgeSurfingUI();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error during urge surfing session");
            await DisplayAlert("Error", "An error occurred during the urge surfing session.", "OK");
            ResetUrgeSurfingUI();
        }
    }

    private void ResetUrgeSurfingUI()
    {
        _isUrgeSurfing = false;
        UrgeSurfingButton.Text = "Start Urge Surfing";
        UrgeSurfingButton.BackgroundColor = Colors.White;
    }

    private async Task StartUrgeSurfingSession(CancellationToken cancellationToken)
    {
        // Phase 1: Acknowledge the urge (30 seconds)
        await DisplayAlert("Step 1: Acknowledge",
            "Take a deep breath. Notice the urge to binge without judging it. \n\n" +
            "Where do you feel it in your body? Is it a tightness, emptiness, or tension?", "Continue");

        if (cancellationToken.IsCancellationRequested) throw new TaskCanceledException();

        // Phase 2: Breathe through it (30 seconds)
        await DisplayAlert("Step 2: Breathe",
            "Take slow, deep breaths. \n\n" +
            "Breathe in for 4 counts, hold for 2, exhale for 6. \n\n" +
            "Continue for 30 seconds.", "Continue");

        if (cancellationToken.IsCancellationRequested) throw new TaskCanceledException();

        // Phase 3: Observe without acting (30 seconds)
        await DisplayAlert("Step 3: Observe",
            "Notice that the urge is like a wave. It rises, peaks, and will eventually fall. \n\n" +
            "You don't have to act on it. Just observe it with curiosity.", "Continue");

        if (cancellationToken.IsCancellationRequested) throw new TaskCanceledException();

        // Phase 4: Remind of values (30 seconds)
        await DisplayAlert("Step 4: Remember Your Why",
            "Remember why you want to overcome binge eating. \n\n" +
            "Connect with your deeper values and goals.", "Continue");

        if (cancellationToken.IsCancellationRequested) throw new TaskCanceledException();

        // Completion
        bool didResistUrge = await DisplayAlert("Urge Surfing Complete",
            "Great job! You've completed the urge surfing exercise. \n\n" +
            "Were you able to resist the urge to binge?", "Yes, I resisted", "No, I binged");

        // Record the result
        await RecordUrgeSurfingResult(didResistUrge);

        // Reset the UI
        ResetUrgeSurfingUI();
    }

    private async Task RecordUrgeSurfingResult(bool didResistUrge)
    {
        try
        {
            if (_dataStore == null) return;

            // Calculate consecutive days without binge
            int consecutiveDaysWithoutBinge = 0;
            if (_dataStore is DatabaseService databaseService)
            {
                var recentEntries = await databaseService.GetBingeEntriesByDateRangeAsync(
                    _username, DateTime.Today.AddDays(-30), DateTime.Today);

                // Group by date and check for days without binges
                var entriesByDate = recentEntries.GroupBy(e => e.Date.Date)
                    .OrderByDescending(g => g.Key)
                    .ToList();

                // Count consecutive days where all entries have WasUrgeResisted = true
                foreach (var group in entriesByDate)
                {
                    if (group.All(e => e.WasUrgeResisted))
                    {
                        consecutiveDaysWithoutBinge++;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            // Calculate points
            int points = BingeEntry.CalculatePoints(didResistUrge, consecutiveDaysWithoutBinge);

            // Create a new entry
            BingeEntry newEntry = new()
            {
                Username = _username,
                Date = DateTime.Now,
                Duration = TimeSpan.FromMinutes(2), // Duration of the urge surfing exercise
                WasUrgeResisted = didResistUrge,
                Points = points,
                Notes = didResistUrge ? "Successfully resisted urge using urge surfing" : "Attempted urge surfing but binged"
            };

            await _dataStore.AddItemAsync(newEntry);

            // Update UI
            if (didResistUrge)
            {
                await DisplayAlert("Success!", $"Congratulations! You earned {points} points for resisting the urge to binge.", "OK");
            }
            else
            {
                // Still add a dot for tracking purposes
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

                await DisplayAlert("Keep Going", $"It's okay. You still earned {points} point for tracking. Each attempt builds your skills.", "OK");
            }

            // Update total points
            _totalPoints += points;
            UpdatePointsLabel();

            Log.Information("Recorded urge surfing result: Resisted={Resisted}, Points={Points}", didResistUrge, points);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error recording urge surfing result");
            await DisplayAlert("Error", "Could not record your result: " + ex.Message, "OK");
        }
    }
}
