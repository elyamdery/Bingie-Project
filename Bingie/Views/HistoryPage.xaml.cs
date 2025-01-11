namespace Bingie.Views;

public partial class HistoryPage : ContentPage
{
    private DateTime _currentDate;

    public HistoryPage()
    {
        InitializeComponent();
        _currentDate = DateTime.Today;
        UpdateCalendar();
    }

    private void UpdateCalendar()
    {
        CurrentDateLabel.Text = _currentDate.ToString("MMMM yyyy");

        // Clear existing grid content
        CalendarGrid.Children.Clear();

        // Get the first day of the current month
        DateTime firstDayOfMonth = new(_currentDate.Year, _currentDate.Month, 1);
        var daysInMonth = DateTime.DaysInMonth(_currentDate.Year, _currentDate.Month);
        var startDayOfWeek = (int)firstDayOfMonth.DayOfWeek;

        // Add the days of the month to the calendar grid
        for (var day = 1; day <= daysInMonth; day++)
        {
            Button dayButton = new()
            {
                Text = day.ToString(),
                BackgroundColor = Colors.Red,
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

    private void OnDaySelected(int day)
    {
        var daysInMonth = DateTime.DaysInMonth(_currentDate.Year, _currentDate.Month);
        if (day < 1 || day > daysInMonth)
        {
            // Handle invalid day value
            Console.WriteLine("Invalid day selected.");
            return;
        }

        DateTime selectedDate = new(_currentDate.Year, _currentDate.Month, day);
        _ = Navigation.PushAsync(new DayStatisticsPage(selectedDate));
    }

    private void OnPreviousWeekClicked(object sender, EventArgs e)
    {
        _currentDate = _currentDate.AddDays(-7);
        UpdateCalendar();
    }

    private void OnNextWeekClicked(object sender, EventArgs e)
    {
        _currentDate = _currentDate.AddDays(7);
        UpdateCalendar();
    }
}