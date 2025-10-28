using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Bingie.Config;
using Bingie.Models;
using Bingie.Services;
using Serilog;

namespace Bingie.Views;

public partial class MainPage : ContentPage
{
    private readonly IDataStore<BingeEntry> _dataStore;
    private readonly CalendarService _calendarService;
    private readonly AvatarFeedbackService _avatarFeedbackService;
    private readonly string _username;
    private bool _suppressAvatarToggle;
    private AvatarFeedbackSnapshot? _latestAvatarSnapshot;

    // Default constructor for XAML preview support
    public MainPage()
    {
        InitializeComponent();
        var connectionFactory = new SqliteConnectionFactory();
        var previewService = new DatabaseService(connectionFactory);
        _dataStore = (IDataStore<BingeEntry>)previewService;
        _calendarService = new CalendarService(_dataStore);
        var avatarRepository = new AvatarFeedbackRepository(connectionFactory);
        _avatarFeedbackService = new AvatarFeedbackService(_dataStore, avatarRepository);
        _username = "PreviewUser";

        StartClock();
        _ = RefreshTodayDotsAsync();
        if (FeatureFlags.AvatarFeedbackEnabled)
        {
            _ = RefreshAvatarFeedbackAsync();
        }
    }

    public MainPage(IDataStore<BingeEntry> dataStore, CalendarService calendarService, AvatarFeedbackService avatarFeedbackService, string username)
    {
        InitializeComponent();
        _dataStore = dataStore ?? throw new ArgumentNullException(nameof(dataStore));
        _calendarService = calendarService ?? throw new ArgumentNullException(nameof(calendarService));
        _avatarFeedbackService = avatarFeedbackService ?? throw new ArgumentNullException(nameof(avatarFeedbackService));
        _username = username ?? throw new ArgumentNullException(nameof(username));

        // Initialize logging in the MainPage as well
        Log.Information("MainPage initialized."); // <-- Log when the MainPage is initialized

        StartClock();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await RefreshTodayDotsAsync();
        await RefreshAvatarFeedbackAsync();
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
            await RefreshAvatarFeedbackAsync();

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

    private async Task RefreshAvatarFeedbackAsync()
    {
        if (!FeatureFlags.AvatarFeedbackEnabled)
        {
            AvatarFeedbackSection.IsVisible = false;
            return;
        }

        try
        {
            AvatarFeedbackSection.IsVisible = true;
            var snapshot = await _avatarFeedbackService.GenerateSnapshotAsync(_username, DateTime.UtcNow);
            _latestAvatarSnapshot = snapshot;

            _suppressAvatarToggle = true;
            AvatarFeedbackToggle.IsToggled = !snapshot.IsHidden;
            _suppressAvatarToggle = false;

            await UpdateAvatarFeedbackUi(snapshot);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to refresh avatar feedback for {Username}.", _username);
            AvatarFeedbackSection.IsVisible = false;
        }
    }

    private async Task UpdateAvatarFeedbackUi(AvatarFeedbackSnapshot snapshot)
    {
        if (snapshot.IsHidden)
        {
            AvatarEnergyTitle.Text = "Avatar resting";
            AvatarScoreLabel.Text = "Feedback is paused";
            AvatarMonthlyLabel.Text = string.Empty;
            AvatarBreakLabel.Text = string.Empty;
            AvatarSupportiveCopy.Text = snapshot.SupportiveCopy;
            AvatarAnimationKeyLabel.Text = snapshot.AnimationKey;
            AvatarCelebrationBadge.IsVisible = false;
            AvatarBackgroundFrame.BackgroundColor = Color.FromArgb("#222845");
            return;
        }

        AvatarEnergyTitle.Text = snapshot.EnergyState switch
        {
            AvatarEnergyState.Energized => "Glow surging",
            AvatarEnergyState.Steady => "Glow steady",
            AvatarEnergyState.Tired => "Glow resting",
            _ => "Avatar energy"
        };

        AvatarScoreLabel.Text = $"Weekly score {snapshot.WeeklyScore:0}";
        AvatarMonthlyLabel.Text = snapshot.MonthlyDelta switch
        {
            > 0 => $"Monthly +{snapshot.MonthlyDelta:0.#}",
            < 0 => $"Monthly {snapshot.MonthlyDelta:0.#}",
            _ => "Monthly steady"
        };

        AvatarBreakLabel.Text = snapshot.CurrentBreakDays is int breakDays and > 0
            ? $"Calm streak: {breakDays} day{(breakDays == 1 ? string.Empty : "s")}"
            : string.Empty;

        AvatarSupportiveCopy.Text = snapshot.SupportiveCopy;
        AvatarAnimationKeyLabel.Text = snapshot.AnimationKey;
        AvatarCelebrationBadge.IsVisible = snapshot.CelebrateMonthlyWin;

        var colors = snapshot.EnergyState switch
        {
            AvatarEnergyState.Energized => Color.FromArgb("#4CFFDF"),
            AvatarEnergyState.Steady => Color.FromArgb("#4D7CFE"),
            AvatarEnergyState.Tired => Color.FromArgb("#FF8F70"),
            _ => Color.FromArgb("#222845")
        };

        AvatarBackgroundFrame.BackgroundColor = colors.WithAlpha(0.32f);

        var targetScale = snapshot.EnergyState switch
        {
            AvatarEnergyState.Energized => 1.08,
            AvatarEnergyState.Steady => 1.03,
            AvatarEnergyState.Tired => 0.98,
            _ => 1.0
        };

        await AvatarBackgroundFrame.ScaleTo(targetScale, 240, Easing.CubicOut);
        await AvatarBackgroundFrame.ScaleTo(1.0, 320, Easing.CubicIn);
    }

    private async void OnAvatarToggleToggled(object sender, ToggledEventArgs e)
    {
        if (_suppressAvatarToggle || _avatarFeedbackService == null) return;

        try
        {
            await _avatarFeedbackService.UpdateHidePreferenceAsync(_username, hideAvatar: !e.Value);
            await RefreshAvatarFeedbackAsync();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to update avatar feedback preference for {Username}.", _username);
            _suppressAvatarToggle = true;
            AvatarFeedbackToggle.IsToggled = e.Value;
            _suppressAvatarToggle = false;
        }
    }
}
