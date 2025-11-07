using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Bingie.Config;
using Bingie.Messaging;
using Bingie.Models;
using Bingie.Services;
using Bingie.Views.Auth;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Storage;
using Serilog;

namespace Bingie.Views;

public partial class MainPage : ContentPage
{
    private const string RememberedUsernameKey = "RememberedUsername";
    private const string RememberTokenKey = "RememberedToken";
    private const string RememberMeFlagKey = "RememberMeEnabled";

    private readonly IDataStore<BingeEntry> _dataStore;
    private readonly CalendarService _calendarService;
    private readonly AvatarFeedbackService _avatarFeedbackService;
    private readonly StoryGuideService? _storyGuideService;
    private readonly PointsSystemService? _pointsSystemService;
    private readonly IAuthService _authService;
    private readonly string _username;
    private readonly IMessenger _messenger = WeakReferenceMessenger.Default;

    private bool _suppressAvatarToggle;
    private bool _settingsPanelVisible;
    private bool _suppressSettingsEvents;
    private bool _suppressAvatarBodySlider;
    private PointsDashboard? _currentPointsSnapshot;
    private AvatarFeedbackSnapshot? _latestAvatarSnapshot;
    private readonly List<AvatarBodyHistoryItem> _avatarBodyHistory = new();

    public MainPage()
    {
        InitializeComponent();
        var connectionFactory = new SqliteConnectionFactory();
        var previewService = new DatabaseService(connectionFactory);
        _dataStore = (IDataStore<BingeEntry>)previewService;
        _calendarService = new CalendarService(_dataStore);
        var avatarRepository = new AvatarFeedbackRepository(connectionFactory);
        _avatarFeedbackService = new AvatarFeedbackService(_dataStore, avatarRepository);
        _authService = new AuthService(previewService);

        if (FeatureFlags.StoryGuideEnabled)
        {
            var storyRepository = new StoryGuideRepository(connectionFactory);
            _storyGuideService = new StoryGuideService(storyRepository, new NoopStoryGuideAnalytics());
        }

        if (FeatureFlags.PointsSystemEnabled)
        {
            var pointsRepository = new PointsSystemRepository(connectionFactory);
            _pointsSystemService = new PointsSystemService(pointsRepository);
        }

        _username = "PreviewUser";

        StartClock();
        _ = RefreshTodayDotsAsync();
        if (FeatureFlags.AvatarFeedbackEnabled)
        {
            _ = RefreshAvatarFeedbackAsync();
        }
        _ = RefreshPointsSummaryAsync();
        UpdateUserStatusUi();
        SyncSettingsPanel();
    }

    public MainPage(
        IDataStore<BingeEntry> dataStore,
        CalendarService calendarService,
        AvatarFeedbackService avatarFeedbackService,
        StoryGuideService storyGuideService,
        PointsSystemService pointsSystemService,
        string username,
        IAuthService authService)
    {
        InitializeComponent();
        _dataStore = dataStore ?? throw new ArgumentNullException(nameof(dataStore));
        _calendarService = calendarService ?? throw new ArgumentNullException(nameof(calendarService));
        _avatarFeedbackService = avatarFeedbackService ?? throw new ArgumentNullException(nameof(avatarFeedbackService));
        _storyGuideService = storyGuideService;
        _pointsSystemService = pointsSystemService;
        _username = username ?? throw new ArgumentNullException(nameof(username));
        _authService = authService ?? throw new ArgumentNullException(nameof(authService));

        Log.Information("MainPage initialized.");
        StartClock();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await RefreshTodayDotsAsync();
        if (FeatureFlags.AvatarFeedbackEnabled)
        {
            await RefreshAvatarFeedbackAsync();
        }
        else
        {
            AvatarFeedbackSection.IsVisible = false;
        }

        await RefreshPointsSummaryAsync();
        await LoadAvatarBodyHistoryAsync();
        UpdateUserStatusUi();
        _messenger.Send(new PointsDashboardUpdatedMessage(_currentPointsSnapshot));
        SyncSettingsPanel();
    }

    private void StartClock()
    {
        Dispatcher.StartTimer(TimeSpan.FromSeconds(1), () =>
        {
            var now = DateTime.UtcNow.AddHours(3);
            var timestamp = now.ToString("HH:mm:ss", CultureInfo.InvariantCulture);
            Log.Information("Current time: {Time}", timestamp);

            CurrentTimeLabel.Text = timestamp;
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
            if (FeatureFlags.AvatarFeedbackEnabled)
            {
                await RefreshAvatarFeedbackAsync();
            }

            _messenger.Send(new BingeEntryAddedMessage(newEntry));

            if (FeatureFlags.PointsSystemEnabled && _pointsSystemService != null)
            {
                try
                {
                    await _pointsSystemService.RecordActionAsync(_username, "log_entry", DateTime.UtcNow);
                }
                catch (Exception xpEx)
                {
                    Log.Warning(xpEx, "Failed to award XP for log entry.");
                }
            }

            await RefreshPointsSummaryAsync();
            await LoadAvatarBodyHistoryAsync();
            UpdateUserStatusUi();
            _messenger.Send(new PointsDashboardUpdatedMessage(_currentPointsSnapshot));

            await ShowFeedbackAsync("Logged! Thanks for checking in 💪");

            if (FeatureFlags.StoryGuideEnabled && _storyGuideService != null)
            {
                await ShowStoryGuidePromptAsync(newEntry);
            }
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
        if (!FeatureFlags.AvatarFeedbackEnabled || _avatarFeedbackService == null)
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

    private async Task RefreshPointsSummaryAsync()
    {
        if (FeatureFlags.PointsSystemEnabled && _pointsSystemService != null)
        {
            try
            {
                _currentPointsSnapshot = await _pointsSystemService.GetDashboardAsync(_username, DateTime.UtcNow);
            }
            catch (Exception ex)
            {
                Log.Warning(ex, "Unable to refresh points dashboard for {Username}.", _username);
                _currentPointsSnapshot = null;
            }
        }
        else
        {
            _currentPointsSnapshot = null;
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
            AvatarBackgroundFrame.BackgroundColor = Color.FromArgb("#1F2445");
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
            AvatarEnergyState.Energized => Color.FromArgb("#3BC8C8"),
            AvatarEnergyState.Steady => Color.FromArgb("#6E7BFF"),
            AvatarEnergyState.Tired => Color.FromArgb("#F28C8C"),
            _ => Color.FromArgb("#1F2445")
        };

        AvatarBackgroundFrame.BackgroundColor = colors.WithAlpha(0.28f);

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

    private async Task ShowStoryGuidePromptAsync(BingeEntry entry)
    {
        if (_storyGuideService is null) return;

        try
        {
            var promptPage = new StoryGuidePromptPage(_storyGuideService, _username, entry.Date);
            await Navigation.PushModalAsync(promptPage);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to launch story guide prompt for {Username}.", _username);
        }
    }

    private void OnSettingsButtonClicked(object sender, EventArgs e)
    {
        _settingsPanelVisible = !_settingsPanelVisible;
        SettingsContainer.IsVisible = _settingsPanelVisible;
        if (_settingsPanelVisible)
        {
            SyncSettingsPanel();
        }
    }

    private async void OnSignOutClicked(object sender, EventArgs e)
    {
        var confirm = await DisplayAlert("Sign out", "Sign out of Bingie?", "Sign out", "Cancel");
        if (!confirm) return;

        try
        {
            await _authService.ClearRememberTokenAsync(new User { Username = _username, Password = string.Empty });
        }
        catch (Exception ex)
        {
            Log.Warning(ex, "Failed to clear remember-me token during sign out for {Username}.", _username);
        }

        Preferences.Remove(RememberedUsernameKey);
        Preferences.Set(RememberMeFlagKey, false);
        SecureStorage.Remove(RememberTokenKey);

        var services = App.Current?.Handler?.MauiContext?.Services;
        var loginPage = services?.GetService<LoginPage>();
        if (loginPage != null)
        {
            Application.Current!.MainPage = new NavigationPage(loginPage)
            {
                BarBackgroundColor = Color.FromArgb("#1F3A93"),
                BarTextColor = Colors.White
            };
        }
    }

    private async void OnAvatarFeatureSwitchToggled(object sender, ToggledEventArgs e)
    {
        if (_suppressSettingsEvents) return;
        FeatureFlags.OverrideAvatarFeedback(e.Value ? (bool?)null : false);

        if (FeatureFlags.AvatarFeedbackEnabled)
        {
            await RefreshAvatarFeedbackAsync();
        }
        else
        {
            AvatarFeedbackSection.IsVisible = false;
        }

        UpdateUserStatusUi();
        SyncSettingsPanel();
    }

    private async void OnStoryFeatureSwitchToggled(object sender, ToggledEventArgs e)
    {
        if (_suppressSettingsEvents) return;
        FeatureFlags.OverrideStoryGuide(e.Value ? (bool?)null : false);
        SyncSettingsPanel();
        await RefreshPointsSummaryAsync();
        UpdateUserStatusUi();
        _messenger.Send(new PointsDashboardUpdatedMessage(_currentPointsSnapshot));
    }

    private async void OnPointsFeatureSwitchToggled(object sender, ToggledEventArgs e)
    {
        if (_suppressSettingsEvents) return;
        FeatureFlags.OverridePointsSystem(e.Value ? (bool?)null : false);
        await RefreshPointsSummaryAsync();
        UpdateUserStatusUi();
        _messenger.Send(new PointsDashboardUpdatedMessage(_currentPointsSnapshot));
        SyncSettingsPanel();
    }

    private async void OnTriggerRadarFeatureSwitchToggled(object sender, ToggledEventArgs e)
    {
        if (_suppressSettingsEvents) return;
        FeatureFlags.OverrideTriggerRadar(e.Value ? (bool?)null : false);
        if (e.Value)
        {
            await DisplayAlert("Trigger radar", "History insights will now show radar-style risk windows based on your logs.", "Nice");
        }
        SyncSettingsPanel();
    }

    private async void OnAvatarBodyFeatureSwitchToggled(object sender, ToggledEventArgs e)
    {
        if (_suppressSettingsEvents) return;
        FeatureFlags.OverrideAvatarBody(e.Value ? (bool?)null : false);
        await LoadAvatarBodyHistoryAsync();
        SyncSettingsPanel();
    }

    private async void OnLeaderboardFeatureSwitchToggled(object sender, ToggledEventArgs e)
    {
        if (_suppressSettingsEvents) return;
        FeatureFlags.OverrideLeaderboard(e.Value);
        _messenger.Send(new FriendsLeaderboardVisibilityChangedMessage(FeatureFlags.LeaderboardEnabled));
        if (e.Value)
        {
            await DisplayAlert("Friends leaderboard", "Friends leaderboard re-enabled. Open the Friends tab to check in with your circle.", "OK");
        }
        SyncSettingsPanel();
    }

    private async void OnLegacyModeSwitchToggled(object sender, ToggledEventArgs e)
    {
        if (_suppressSettingsEvents) return;

        if (e.Value)
        {
            FeatureFlags.OverrideAvatarFeedback(false);
            FeatureFlags.OverrideStoryGuide(false);
            FeatureFlags.OverridePointsSystem(false);
            FeatureFlags.OverrideTriggerRadar(false);
            FeatureFlags.OverrideAvatarBody(false);
            FeatureFlags.OverrideLeaderboard(false);
        }
        else
        {
            FeatureFlags.OverrideAvatarFeedback(null);
            FeatureFlags.OverrideStoryGuide(null);
            FeatureFlags.OverridePointsSystem(null);
            FeatureFlags.OverrideTriggerRadar(null);
            FeatureFlags.OverrideAvatarBody(null);
            FeatureFlags.OverrideLeaderboard(null);
        }

        SyncSettingsPanel();

        if (FeatureFlags.AvatarFeedbackEnabled)
        {
            await RefreshAvatarFeedbackAsync();
        }
        else
        {
            AvatarFeedbackSection.IsVisible = false;
        }

        await RefreshPointsSummaryAsync();
        UpdateUserStatusUi();
        _messenger.Send(new PointsDashboardUpdatedMessage(_currentPointsSnapshot));
    }

    private void SyncSettingsPanel()
    {
        _suppressSettingsEvents = true;
        AvatarFeatureSwitch.IsToggled = FeatureFlags.AvatarFeedbackEnabled;
        StoryFeatureSwitch.IsToggled = FeatureFlags.StoryGuideEnabled;
        PointsFeatureSwitch.IsToggled = FeatureFlags.PointsSystemEnabled;
        TriggerRadarFeatureSwitch.IsToggled = FeatureFlags.TriggerRadarEnabled;
        AvatarBodyFeatureSwitch.IsToggled = FeatureFlags.AvatarBodyEnabled;
        LeaderboardFeatureSwitch.IsToggled = FeatureFlags.LeaderboardEnabled;
        LegacyModeSwitch.IsToggled = !FeatureFlags.AvatarFeedbackEnabled &&
                                     !FeatureFlags.StoryGuideEnabled &&
                                     !FeatureFlags.PointsSystemEnabled &&
                                     !FeatureFlags.TriggerRadarEnabled &&
                                     !FeatureFlags.AvatarBodyEnabled &&
                                     !FeatureFlags.LeaderboardEnabled;
        _suppressSettingsEvents = false;
    }

    private async Task LoadAvatarBodyHistoryAsync()
    {
        if (!FeatureFlags.AvatarBodyEnabled)
        {
            AvatarBodySection.IsVisible = false;
            return;
        }

        List<AvatarBodyHistoryItem> items = new();
        var today = DateTime.Today;
        var tasks = Enumerable.Range(0, 7)
            .Select(offset => _calendarService.GetEntriesForDateAsync(_username, today.AddDays(-offset)))
            .ToList();

        var results = await Task.WhenAll(tasks);
        for (var i = 0; i < results.Length; i++)
        {
            var date = today.AddDays(-i);
            var entryCount = results[i].Count;
            var score = Math.Clamp(1 - (entryCount / 4d), 0.2, 1.2);
            var copy = ComposeAvatarBodyCopy(entryCount);
            items.Add(new AvatarBodyHistoryItem(date, score, copy));
        }

        items = items.OrderBy(item => item.Date).ToList();
        _avatarBodyHistory.Clear();
        _avatarBodyHistory.AddRange(items);

        if (_avatarBodyHistory.Count == 0)
        {
            AvatarBodySection.IsVisible = false;
            return;
        }

        AvatarBodySection.IsVisible = true;
        AvatarBodyHistoryView.ItemsSource = _avatarBodyHistory
            .Select(item => new AvatarBodyHistoryListItem(item))
            .ToList();

        _suppressAvatarBodySlider = true;
        AvatarBodySlider.Maximum = _avatarBodyHistory.Count - 1;
        AvatarBodySlider.Minimum = 0;
        AvatarBodySlider.Value = _avatarBodyHistory.Count - 1;
        _suppressAvatarBodySlider = false;

        ApplyAvatarBodySelection(_avatarBodyHistory.Count - 1);
    }

    private void OnAvatarBodySliderChanged(object sender, ValueChangedEventArgs e)
    {
        if (_suppressAvatarBodySlider) return;
        ApplyAvatarBodySelection((int)Math.Round(e.NewValue));
    }

    private void ApplyAvatarBodySelection(int index)
    {
        if (_avatarBodyHistory.Count == 0) return;
        index = Math.Clamp(index, 0, _avatarBodyHistory.Count - 1);
        var selected = _avatarBodyHistory[index];
        AvatarBodyView.BodyScore = selected.Score;
        AvatarBodyView.MoodCopy = selected.Copy;
        AvatarBodyDateLabel.Text = selected.Date.Date == DateTime.Today
            ? "Today"
            : selected.Date.ToString("ddd, MMM d", CultureInfo.InvariantCulture);
    }

    private static string ComposeAvatarBodyCopy(int entryCount)
    {
        return entryCount switch
        {
            0 => "Glow strong|Keep stacking gentle habits.",
            1 => "Steady arc|Notice urges and name them.",
            2 => "Avatar wobbly|Prep a snack or text support tonight.",
            _ => "Avatar tired|Plan a cozy ritual ASAP."
        };
    }

    private void UpdateUserStatusUi()
    {
        UserNameLabel.Text = _username;

        if (FeatureFlags.PointsSystemEnabled && _currentPointsSnapshot != null)
        {
            UserLevelLabel.Text = $"Level {_currentPointsSnapshot.GlowLevel}";
            UserXpLabel.Text = $"{_currentPointsSnapshot.TotalXp} XP · This week {_currentPointsSnapshot.WeeklyXp} XP";
        }
        else
        {
            UserLevelLabel.Text = "Legacy mode";
            UserXpLabel.Text = "Advanced features are paused";
        }
    }

    private sealed record AvatarBodyHistoryItem(DateTime Date, double Score, string Copy)
    {
        public string DateLabel => Date.ToString("ddd", CultureInfo.InvariantCulture);
    }

    private sealed record AvatarBodyHistoryListItem(AvatarBodyHistoryItem Source)
    {
        public string DateLabel => Source.DateLabel;
        public double ScoreNormalized => Math.Clamp(Source.Score, 0, 1);
    }
}
