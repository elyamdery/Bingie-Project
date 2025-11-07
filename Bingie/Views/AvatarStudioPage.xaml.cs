using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Bingie.Services;

namespace Bingie.Views;

public partial class AvatarStudioPage : ContentPage
{
    private readonly CalendarService _calendarService;
    private readonly string _username;
    private readonly List<AvatarBodyHistoryItem> _history = new();
    private bool _suppressSlider;

    public AvatarStudioPage(CalendarService calendarService, string username)
    {
        InitializeComponent();
        _calendarService = calendarService ?? throw new ArgumentNullException(nameof(calendarService));
        _username = username ?? throw new ArgumentNullException(nameof(username));
        _ = LoadAsync();
    }

    private async Task LoadAsync()
    {
        var today = DateTime.Today;
        for (var i = 0; i < 7; i++)
        {
            var date = today.AddDays(-i);
            var entries = await _calendarService.GetEntriesForDateAsync(_username, date);
            var score = Math.Clamp(1 - (entries.Count / 4d), 0.2, 1.2);
            _history.Add(new AvatarBodyHistoryItem(date, score));
        }

        _history.Reverse();
        StudioAvatarHistory.ItemsSource = _history
            .Select(item => new AvatarBodyHistoryListItem(item))
            .ToList();

        _suppressSlider = true;
        StudioAvatarSlider.Maximum = _history.Count - 1;
        StudioAvatarSlider.Value = _history.Count - 1;
        _suppressSlider = false;
        ApplySelection(_history.Count - 1);
    }

    private void OnSliderChanged(object sender, ValueChangedEventArgs e)
    {
        if (_suppressSlider) return;
        ApplySelection((int)Math.Round(e.NewValue));
    }

    private void ApplySelection(int index)
    {
        if (_history.Count == 0) return;
        index = Math.Clamp(index, 0, _history.Count - 1);
        var selected = _history[index];
        StudioAvatarBody.BodyScore = selected.Score;
        StudioAvatarBody.MoodCopy = selected.Copy;
        StudioAvatarDateLabel.Text = selected.Date == DateTime.Today
            ? "Today"
            : selected.Date.ToString("ddd, MMM d", CultureInfo.InvariantCulture);
    }

    private sealed record AvatarBodyHistoryItem(DateTime Date, double Score)
    {
        public string DateLabel => Date.ToString("ddd", CultureInfo.InvariantCulture);
        public double ScoreNormalized => Math.Clamp(Score, 0, 1);
        public string Copy => Score switch
        {
            > 0.9 => "Glow lifted|Celebrate how you cared.",
            > 0.7 => "Steady arc|Keep stacking wins.",
            > 0.5 => "Avatar wobbly|Try a grounding ritual.",
            _ => "Avatar tired|Offer water, rest, or support."
        };
    }

    private sealed record AvatarBodyHistoryListItem(AvatarBodyHistoryItem Source)
    {
        public string DateLabel => Source.DateLabel;
        public double ScoreNormalized => Source.ScoreNormalized;
    }
}
