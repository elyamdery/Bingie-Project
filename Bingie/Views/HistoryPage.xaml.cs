using System;
using Microsoft.Maui.Controls;

namespace Bingie.Views
{
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
            var firstDayOfMonth = new DateTime(_currentDate.Year, _currentDate.Month, 1);
            int daysInMonth = DateTime.DaysInMonth(_currentDate.Year, _currentDate.Month);
            int startDayOfWeek = (int)firstDayOfMonth.DayOfWeek;

            // Add the days of the month to the calendar grid
            for (int day = 1; day <= daysInMonth; day++)
            {
                var dayButton = new Button
                {
                    Text = day.ToString(),
                    BackgroundColor = Colors.Red,
                    TextColor = Colors.White,
                    CornerRadius = 20,
                    HeightRequest = 40,
                    WidthRequest = 40
                };

                int row = (startDayOfWeek + day - 1) / 7;
                int column = (startDayOfWeek + day - 1) % 7;

                dayButton.Clicked += (s, e) => OnDaySelected(day);

                CalendarGrid.Add(dayButton, column, row);
            }
        }

        private void OnDaySelected(int day)
        {
            int daysInMonth = DateTime.DaysInMonth(_currentDate.Year, _currentDate.Month);
            if (day < 1 || day > daysInMonth)
            {
                // Handle invalid day value
                Console.WriteLine("Invalid day selected.");
                return;
            }

            var selectedDate = new DateTime(_currentDate.Year, _currentDate.Month, day);
            Navigation.PushAsync(new DayStatisticsPage(selectedDate));
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
}