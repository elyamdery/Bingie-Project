using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Bingie.Config;
using Bingie.Constants;
using Bingie.Messaging;
using Bingie.Models;
using Bingie.Services;
using Bingie.Views.Components;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Maui.ApplicationModel.DataTransfer;

namespace Bingie.Views;

public partial class HistoryPage : ContentPage
{
    private readonly BingeTrendDrawable _trendDrawable = new();
    private readonly TriggerRadarDrawable _radarDrawable = new();
    private readonly IDataStore<BingeEntry> _dataStore;
    private readonly CalendarService _calendarService;
    private readonly TriggerRadarService? _triggerRadarService;
    private readonly string _username;
    private DateTime _currentMonth;
    private bool _isLoading;
    private bool _analyticsExpanded;
    private TriggerRadarResult? _latestTriggerRadar;
    private readonly IMessenger _messenger = WeakReferenceMessenger.Default;
    private bool _isSubscribed;

    public HistoryPage()
    {
        InitializeComponent();

        var connectionFactory = new SqliteConnectionFactory();
        var previewService = new DatabaseService(connectionFactory);
        _dataStore = (IDataStore<BingeEntry>)previewService;
        _calendarService = new CalendarService(_dataStore);
        _username = "PreviewUser";
        _currentMonth = DateTime.Today;
        _triggerRadarService = new TriggerRadarService((IDataStore<BingeEntry>)previewService);

        BingeChartView.Drawable = _trendDrawable;
        CalendarView.DayTapped += OnDayTapped;
        TriggerRadarView.Drawable = _radarDrawable;
    }

    public HistoryPage(
        IDataStore<BingeEntry> dataStore,
        CalendarService calendarService,
        TriggerRadarService triggerRadarService,
        string username)
    {
        InitializeComponent();
        _dataStore = dataStore ?? throw new ArgumentNullException(nameof(dataStore));
        _calendarService = calendarService ?? throw new ArgumentNullException(nameof(calendarService));
        _triggerRadarService = triggerRadarService;
        _username = username ?? throw new ArgumentNullException(nameof(username));
        _currentMonth = DateTime.Today;

        BingeChartView.Drawable = _trendDrawable;
        CalendarView.DayTapped += OnDayTapped;
        TriggerRadarView.Drawable = _radarDrawable;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (!_isSubscribed)
        {
            _messenger.Register<BingeEntryAddedMessage>(this, async (_, __) => await UpdateCalendarAsync());
            _isSubscribed = true;
        }
        await UpdateCalendarAsync();
    }

    protected override void OnDisappearing()
    {
        if (_isSubscribed)
        {
            _messenger.Unregister<BingeEntryAddedMessage>(this);
            _isSubscribed = false;
        }
        base.OnDisappearing();
    }

    private async Task UpdateCalendarAsync()
    {
        if (_isLoading) return;

        try
        {
            _isLoading = true;
            LoadingIndicator.IsVisible = LoadingIndicator.IsRunning = true;

            CurrentDateLabel.Text = _currentMonth.ToString(CalendarConstants.MonthYearFormat);
            CalendarView.Month = _currentMonth;

            var bingeCounts = await _calendarService.GetMonthlyBingeCountsAsync(_username, _currentMonth);
            CalendarView.BingeCounts = bingeCounts;

            await UpdateAnalyticsAsync();
            await RefreshTriggerRadarAsync();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"HistoryPage: failed to load calendar data - {ex.Message}");
            await DisplayAlert("Error", "Unable to load calendar data.", "OK");
        }
        finally
        {
            LoadingIndicator.IsRunning = false;
            LoadingIndicator.IsVisible = false;
            _isLoading = false;
        }
    }

    private async void OnPreviousMonthClicked(object sender, EventArgs e)
    {
        _currentMonth = _currentMonth.AddMonths(-1);
        await UpdateCalendarAsync();
    }

    private async void OnNextMonthClicked(object sender, EventArgs e)
    {
        _currentMonth = _currentMonth.AddMonths(1);
        await UpdateCalendarAsync();
    }

    private async void OnDayTapped(object? sender, DateSelectedEventArgs e)
    {
        try
        {
            await Navigation.PushAsync(new DayStatisticsPage(e.Date, _calendarService, _username));
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"HistoryPage: navigation failed - {ex.Message}");
        }
    }

    private async Task UpdateAnalyticsAsync()
    {
        try
        {
            IEnumerable<BingeEntry> entries = await _dataStore.GetItemsAsync();

            List<BingeEntry> userEntries = entries
                .Where(r => string.Equals(r.Username, _username, StringComparison.OrdinalIgnoreCase))
                .OrderBy(r => r.Date)
                .ToList();

            UpdateSummary(userEntries);
            UpdateTrend(userEntries);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"HistoryPage: analytics update failed - {ex.Message}");
        }
    }

    private void UpdateSummary(IReadOnlyCollection<BingeEntry> entries)
    {
        var today = DateTime.Today;
        var monthlyCount = BingeAnalytics.CountThisMonth(entries, today);

        SummaryCountLabel.Text = $"{monthlyCount} logged this month";

        var breakDays = BingeAnalytics.CurrentBreakDays(entries, today);

        if (breakDays is null)
        {
            SummaryStreakLabel.Text = "Current break: day one — keep going!";
            return;
        }

        SummaryStreakLabel.Text = breakDays.Value switch
        {
            < 0 => "Current break: today is a reset.",
            0 => "Current break: today is a reset.",
            1 => "Current break: 1 day strong 💫",
            _ => $"Current break: {breakDays.Value} days strong"
        };
    }

    private void UpdateTrend(IReadOnlyCollection<BingeEntry> entries)
    {
        if (entries.Count == 0)
        {
            _trendDrawable.UpdateData(Array.Empty<BingeDayStat>());
            BingeChartView.Invalidate();
            AnalyticsCollectionView.ItemsSource = Array.Empty<AnalyticsRow>();
            AnalyticsContent.IsVisible = false;
            return;
        }

        var stats = BingeAnalytics.BuildTrend(entries, DateTime.Today, 7, 42);

        _trendDrawable.UpdateData(stats);
        BingeChartView.Invalidate();

        var recent = stats.TakeLast(7).Reverse()
            .Select(stat =>
            {
                var headline = stat.Count == 0
                    ? "Rest day — no binges logged."
                    : $"{stat.Count} logged moment(s).";

                var detail = $"7-day avg: {stat.MovingAverage:F1}";

                return new AnalyticsRow(
                    stat.Date.ToString("ddd, MMM d"),
                    $"{headline}\n{detail}",
                    stat.Count.ToString(),
                    stat.MovingAverage);
            })
            .ToList();

        AnalyticsCollectionView.ItemsSource = recent;
        AnalyticsContent.IsVisible = _analyticsExpanded;
    }

    private sealed record AnalyticsRow(string DayDisplay, string Summary, string CountDisplay, double MovingAverage);

    private void OnToggleAnalyticsClicked(object sender, EventArgs e)
    {
        _analyticsExpanded = !_analyticsExpanded;
        AnalyticsContent.IsVisible = _analyticsExpanded;
        ToggleAnalyticsButton.Text = _analyticsExpanded ? "Hide trend insights" : "Show trend insights";
    }

    private async Task RefreshTriggerRadarAsync()
    {
        if (!FeatureFlags.TriggerRadarEnabled || _triggerRadarService == null)
        {
            TriggerRadarContainer.IsVisible = false;
            return;
        }

        try
        {
            var result = await _triggerRadarService.GetAsync(_username, DateTime.UtcNow);
            _latestTriggerRadar = result;

            if (result == null)
            {
                TriggerRadarContainer.IsVisible = false;
                return;
            }

            TriggerRadarContainer.IsVisible = true;
            TriggerRadarSummaryLabel.Text = result.Summary;
            TriggerRadarSuggestionsView.ItemsSource = result.Suggestions;
            TriggerRadarEmptyLabel.IsVisible = result.TotalEvents < 3;

            if (result.TimeOfDayBuckets.Count > 0)
            {
                _radarDrawable.Buckets = result.TimeOfDayBuckets;
                TriggerRadarView.Invalidate();
                TriggerRadarView.IsVisible = true;
            }
            else
            {
                TriggerRadarView.IsVisible = false;
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"HistoryPage: trigger radar failed - {ex.Message}");
            TriggerRadarContainer.IsVisible = false;
        }
    }

    private async void OnShareTriggerRadarClicked(object sender, EventArgs e)
    {
        if (_latestTriggerRadar == null)
        {
            await DisplayAlert("Trigger radar", "No radar insights yet. Keep logging to build a snapshot.", "OK");
            return;
        }

        var export = $"Trigger radar summary:\n{_latestTriggerRadar.Summary}\n\nSuggestions:\n- {string.Join("\n- ", _latestTriggerRadar.Suggestions)}";
        await Clipboard.SetTextAsync(export);
        await DisplayAlert("Trigger radar", "Snapshot copied to clipboard. Share it with your support person whenever you're ready.", "Great");
    }
}
