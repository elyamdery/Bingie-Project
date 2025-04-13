using System.Diagnostics;
using Bingie.Models;
using Bingie.Services;

namespace Bingie.Views;

public partial class HistoryPage : ContentPage
{
    private readonly IDataStore<BingeEntry> _dataStore;
    private readonly string _username;
    private DateTime _currentDate;
    private int _currentWeek;

    public HistoryPage()
    {
        InitializeComponent();
        _dataStore = new DataStore<BingeEntry>(new SqliteConnectionFactory());
        _username = "PreviewUser";
        _currentDate = DateTime.Today; // Start from today
        _currentWeek = GetWeekOfYear(_currentDate);

        try
        {
            Debug.WriteLine("HistoryPage: Initializing UpdateCalendar");
            UpdateCalendar();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Exception in HistoryPage constructor: {ex.Message}");
            throw;
        }
    }

    public HistoryPage(IDataStore<BingeEntry> dataStore, string username)
    {
        InitializeComponent();
        _dataStore = dataStore ?? throw new ArgumentNullException(nameof(dataStore));
        _username = username ?? throw new ArgumentNullException(nameof(username));
        _currentDate = DateTime.Today; // Start from today
        _currentWeek = GetWeekOfYear(_currentDate);

        try
        {
            Debug.WriteLine("HistoryPage: Initializing UpdateCalendar");
            UpdateCalendar();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Exception in HistoryPage constructor: {ex.Message}");
            throw;
        }
    }

    private async void UpdateCalendar()
    {
        if (_dataStore == null)
        {
            Debug.WriteLine("HistoryPage: _dataStore is null");
            return;
        }

        try
        {
            Debug.WriteLine("HistoryPage: Updating calendar");

            // Get the first day of the current week
            DateTime firstDayOfWeek = _currentDate.AddDays(-(int)_currentDate.DayOfWeek);
            DateTime lastDayOfWeek = firstDayOfWeek.AddDays(6);

            // Update week labels
            WeekNumberLabel.Text = $"Week {_currentWeek}";
            DateRangeLabel.Text = $"{firstDayOfWeek:MMM d} - {lastDayOfWeek:MMM d, yyyy}";

            // Clear existing grid content
            CalendarGrid.Children.Clear();

            var daysInWeek = 7;

            // Get all binge records for the user
            IEnumerable<BingeEntry> allRecords = await _dataStore.GetItemsAsync();
            var userRecords = allRecords.Where(r => r.Username == _username).ToList();

            // Get binge counts for the current week
            var bingeCounts = new Dictionary<DateTime, int>();

            // Group by full date
            foreach (var record in userRecords)
            {
                if (record.Date.Date >= firstDayOfWeek.Date && record.Date.Date < firstDayOfWeek.AddDays(daysInWeek).Date)
                {
                    if (bingeCounts.ContainsKey(record.Date.Date))
                        bingeCounts[record.Date.Date]++;
                    else
                        bingeCounts[record.Date.Date] = 1;
                }
            }

            // Add the days of the week to the calendar grid
            for (var day = 0; day < daysInWeek; day++)
            {
                DateTime currentDay = firstDayOfWeek.AddDays(day);
                bool isToday = currentDay.Date == DateTime.Today;
                int bingeCount = bingeCounts.ContainsKey(currentDay.Date) ? bingeCounts[currentDay.Date] : 0;

                // Create a frame to hold the day content
                Frame dayFrame = new()
                {
                    BackgroundColor = isToday ? "#e9ecef" : "#f8f9fa",
                    BorderColor = isToday ? "#4dabf7" : "#dee2e6",
                    CornerRadius = 10,
                    Padding = new Thickness(5),
                    HasShadow = isToday,
                    HeightRequest = 80
                };

                // Create a grid for the day content
                Grid dayGrid = new()
                {
                    RowDefinitions = new RowDefinitionCollection
                    {
                        new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) },
                        new RowDefinition { Height = new GridLength(1, GridUnitType.Star) }
                    }
                };

                // Day number
                Label dayLabel = new()
                {
                    Text = currentDay.Day.ToString(),
                    FontSize = 18,
                    FontAttributes = isToday ? FontAttributes.Bold : FontAttributes.None,
                    TextColor = isToday ? "#343a40" : "#6c757d",
                    HorizontalOptions = LayoutOptions.Center
                };

                // Binge indicator
                StackLayout indicatorLayout = new()
                {
                    Orientation = StackOrientation.Horizontal,
                    HorizontalOptions = LayoutOptions.Center,
                    Spacing = 2
                };

                // Add dots for binge count
                for (int i = 0; i < Math.Min(bingeCount, 5); i++)
                {
                    Frame dot = new()
                    {
                        BackgroundColor = Colors.Red,
                        WidthRequest = 8,
                        HeightRequest = 8,
                        CornerRadius = 4,
                        Padding = 0,
                        Margin = new Thickness(1)
                    };
                    indicatorLayout.Children.Add(dot);
                }

                // If more than 5 binges, add a count label
                if (bingeCount > 5)
                {
                    Label countLabel = new()
                    {
                        Text = $"+{bingeCount - 5}",
                        FontSize = 10,
                        TextColor = Colors.Red,
                        Margin = new Thickness(2, 0, 0, 0)
                    };
                    indicatorLayout.Children.Add(countLabel);
                }

                // Add elements to the grid
                dayGrid.Add(dayLabel, 0, 0);
                dayGrid.Add(indicatorLayout, 0, 1);
                dayFrame.Content = dayGrid;

                // Make the frame tappable
                TapGestureRecognizer tapGesture = new();
                tapGesture.Tapped += (s, e) => OnDaySelected(currentDay);
                dayFrame.GestureRecognizers.Add(tapGesture);

                // Add to calendar grid
                CalendarGrid.Add(dayFrame, day, 0);
            }

            // Update monthly statistics
            UpdateMonthlyStatistics(userRecords);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Exception in UpdateCalendar: {ex.Message}");
            await DisplayAlert("Error", $"Failed to update calendar: {ex.Message}", "OK");
        }
    }

    private void UpdateMonthlyStatistics(List<BingeEntry> userRecords)
    {
        try
        {
            // Get current month records
            var currentMonth = _currentDate.Month;
            var currentYear = _currentDate.Year;
            var monthStart = new DateTime(currentYear, currentMonth, 1);
            var monthEnd = monthStart.AddMonths(1);

            var monthlyRecords = userRecords.Where(r =>
                r.Date >= monthStart &&
                r.Date < monthEnd).ToList();

            // Total episodes this month
            int totalEpisodes = monthlyRecords.Count;
            TotalEpisodesLabel.Text = totalEpisodes.ToString();

            // Calculate daily average
            int daysInMonth = DateTime.DaysInMonth(currentYear, currentMonth);
            int daysElapsed = Math.Min(DateTime.Today.Day, daysInMonth);
            double average = totalEpisodes / (double)daysElapsed;
            AveragePerDayLabel.Text = average.ToString("F1");

            // Find most active day
            if (monthlyRecords.Any())
            {
                var groupedByDay = monthlyRecords
                    .GroupBy(r => r.Date.Date)
                    .Select(g => new { Date = g.Key, Count = g.Count() })
                    .OrderByDescending(x => x.Count)
                    .FirstOrDefault();

                if (groupedByDay != null)
                {
                    MostActiveDayLabel.Text = $"{groupedByDay.Date:MMM d} ({groupedByDay.Count})";
                }
                else
                {
                    MostActiveDayLabel.Text = "-";
                }

                // Calculate trend (comparing first half to second half of month)
                var midMonth = new DateTime(currentYear, currentMonth, 15);
                var firstHalfCount = monthlyRecords.Count(r => r.Date < midMonth);
                var secondHalfCount = monthlyRecords.Count(r => r.Date >= midMonth);

                if (firstHalfCount > 0 || secondHalfCount > 0)
                {
                    if (secondHalfCount < firstHalfCount)
                    {
                        TrendLabel.Text = "Improving ↓";
                        TrendLabel.TextColor = Colors.Green;
                    }
                    else if (secondHalfCount > firstHalfCount)
                    {
                        TrendLabel.Text = "Increasing ↑";
                        TrendLabel.TextColor = Colors.Red;
                    }
                    else
                    {
                        TrendLabel.Text = "Stable →";
                        TrendLabel.TextColor = Colors.Orange;
                    }
                }
                else
                {
                    TrendLabel.Text = "-";
                    TrendLabel.TextColor = Colors.Gray;
                }
            }
            else
            {
                MostActiveDayLabel.Text = "-";
                TrendLabel.Text = "-";
                TrendLabel.TextColor = Colors.Gray;
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Exception in UpdateMonthlyStatistics: {ex.Message}");
            // Don't throw here, just log the error
        }
    }

    private void OnDaySelected(DateTime selectedDate)
    {
        try
        {
            Debug.WriteLine($"HistoryPage: Day selected: {selectedDate}");
            Debug.WriteLine($"Navigation Stack Count Before: {Navigation.NavigationStack.Count}");
            _ = Navigation.PushAsync(new DayStatisticsPage(selectedDate, _dataStore, _username));
            Debug.WriteLine($"Navigation Stack Count After: {Navigation.NavigationStack.Count}");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Exception in OnDaySelected: {ex.Message}");
            throw;
        }
    }

    private void OnPreviousWeekClicked(object sender, EventArgs e)
    {
        try
        {
            Debug.WriteLine("HistoryPage: Previous week clicked");
            DateTime previousWeekDate = _currentDate.AddDays(-7);
            // Allow navigation to any previous week
            _currentDate = previousWeekDate;
            _currentWeek = GetWeekOfYear(_currentDate);
            UpdateCalendar();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Exception in OnPreviousWeekClicked: {ex.Message}");
            throw;
        }
    }

    private void OnNextWeekClicked(object sender, EventArgs e)
    {
        try
        {
            Debug.WriteLine("HistoryPage: Next week clicked");
            DateTime nextWeekDate = _currentDate.AddDays(7);
            // Only allow navigation up to the current week
            if (nextWeekDate <= DateTime.Today.AddDays(7))
            {
                _currentDate = nextWeekDate;
                _currentWeek = GetWeekOfYear(_currentDate);
                UpdateCalendar();
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Exception in OnNextWeekClicked: {ex.Message}");
            throw;
        }
    }

    private void OnShowCurrentDayStatsClicked(object sender, EventArgs e)
    {
        try
        {
            Debug.WriteLine("HistoryPage: Show current day stats clicked");
            DateTime today = DateTime.Today;
            _ = Navigation.PushAsync(new DayStatisticsPage(today, _dataStore, _username));
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Exception in OnShowCurrentDayStatsClicked: {ex.Message}");
            throw;
        }
    }

    private int GetWeekOfYear(DateTime date)
    {
        var culture = System.Globalization.CultureInfo.CurrentCulture;
        var calendar = culture.Calendar;
        var weekRule = culture.DateTimeFormat.CalendarWeekRule;
        var firstDayOfWeek = culture.DateTimeFormat.FirstDayOfWeek;
        return calendar.GetWeekOfYear(date, weekRule, firstDayOfWeek);
    }
}
