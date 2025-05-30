using System.Diagnostics;
using Bingie.Models;
using Bingie.Services;

namespace Bingie.Views;

public partial class HistoryPage : ContentPage
{
    private readonly IDataStore<BingeEntry> _dataStore;
    private readonly string _username;
    private DateTime _currentDate;

    // Parameterless constructor for XAML previewer
    public HistoryPage()
    {
        InitializeComponent();
        _dataStore = new DataStore<BingeEntry>(new SqliteConnectionFactory()); // Use actual implementation
        _username = "PreviewUser";
        _currentDate = DateTime.Today;

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
        _currentDate = DateTime.Today;

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
            CurrentDateLabel.Text = _currentDate.ToString("MMMM yyyy");

            // Clear existing grid content
            CalendarGrid.Children.Clear();

            // Get the first day of the current month
            DateTime firstDayOfMonth = new(_currentDate.Year, _currentDate.Month, 1);
            var daysInMonth = DateTime.DaysInMonth(_currentDate.Year, _currentDate.Month);
            var startDayOfWeek = (int)firstDayOfMonth.DayOfWeek;

            // Get binge counts for the current month
            IEnumerable<BingeEntry> records = await _dataStore.GetItemsAsync();
            var bingeCounts = records
                .Where(r => r.Username == _username && r.Date.Year == _currentDate.Year &&
                            r.Date.Month == _currentDate.Month)
                .GroupBy(r => r.Date.Day)
                .ToDictionary(g => g.Key, g => g.Count());

            // Add the days of the month to the calendar grid
            for (var day = 1; day <= daysInMonth; day++)
            {
                Button dayButton = new()
                {
                    Text = day.ToString(),
                    BackgroundColor = bingeCounts.ContainsKey(day) ? Colors.Red : Colors.Gray,
                    TextColor = Colors.White,
                    CornerRadius = 20,
                    HeightRequest = 40,
                    WidthRequest = 40
                };

                var row = (startDayOfWeek + day - 1) / 7;
                var column = (startDayOfWeek + day - 1) % 7;

                dayButton.Clicked += (s, e) => OnDaySelected(day);

                CalendarGrid.Add(dayButton, column, row);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Exception in UpdateCalendar: {ex.Message}");
            throw;
        }
    }

    private void OnDaySelected(int day)
    {
        try
        {
            Debug.WriteLine($"HistoryPage: Day selected: {day}");
            var daysInMonth = DateTime.DaysInMonth(_currentDate.Year, _currentDate.Month);
            if (day < 1 || day > daysInMonth)
            {
                // Handle invalid day value
                Console.WriteLine("Invalid day selected.");
                return;
            }

            DateTime selectedDate = new(_currentDate.Year, _currentDate.Month, day);
            _ = Navigation.PushAsync(new DayStatisticsPage(selectedDate, _dataStore, _username));
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
            _currentDate = _currentDate.AddDays(-7);
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
            _currentDate = _currentDate.AddDays(7);
            UpdateCalendar();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Exception in OnNextWeekClicked: {ex.Message}");
            throw;
        }
    }
}