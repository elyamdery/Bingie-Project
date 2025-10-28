using System;
using System.Collections.Generic;
using System.Globalization;
using Bingie.Constants;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Bingie.Views.Components;

public sealed class DateSelectedEventArgs : EventArgs
{
    public DateSelectedEventArgs(DateTime date)
    {
        Date = date;
    }

    public DateTime Date { get; }
}

public class CalendarView : ContentView
{
    public static readonly BindableProperty MonthProperty = BindableProperty.Create(
        nameof(Month),
        typeof(DateTime),
        typeof(CalendarView),
        DateTime.Today,
        propertyChanged: OnCalendarPropertyChanged);

    public static readonly BindableProperty BingeCountsProperty = BindableProperty.Create(
        nameof(BingeCounts),
        typeof(IDictionary<int, int>),
        typeof(CalendarView),
        null,
        propertyChanged: OnCalendarPropertyChanged);

    private readonly Grid _calendarGrid;

    public CalendarView()
    {
        _calendarGrid = new Grid
        {
            ColumnSpacing = 8,
            RowSpacing = 8,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Start
        };

        _calendarGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        for (var i = 0; i < 6; i++)
            _calendarGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

        for (var i = 0; i < CalendarConstants.DaysPerWeek; i++)
            _calendarGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });

        Content = _calendarGrid;

        RenderCalendar();
    }

    public DateTime Month
    {
        get => (DateTime)GetValue(MonthProperty);
        set => SetValue(MonthProperty, value);
    }

    public IDictionary<int, int>? BingeCounts
    {
        get => (IDictionary<int, int>?)GetValue(BingeCountsProperty);
        set => SetValue(BingeCountsProperty, value);
    }

    public event EventHandler<DateSelectedEventArgs>? DayTapped;

    private static void OnCalendarPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is CalendarView view)
        {
            view.RenderCalendar();
        }
    }

    private void RenderCalendar()
    {
        if (_calendarGrid == null) return;

        _calendarGrid.Children.Clear();

        var dayNames = CultureInfo.CurrentCulture.DateTimeFormat.AbbreviatedDayNames;
        for (var column = 0; column < CalendarConstants.DaysPerWeek; column++)
        {
            Label headerLabel = new()
            {
                Text = dayNames[column],
                HorizontalTextAlignment = TextAlignment.Center,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.FromArgb(CalendarConstants.NormalDayTextColor)
            };

            _calendarGrid.Add(headerLabel, column, 0);
        }

        var firstDayOfMonth = new DateTime(Month.Year, Month.Month, 1);
        var daysInMonth = DateTime.DaysInMonth(Month.Year, Month.Month);
        var startOffset = (int)firstDayOfMonth.DayOfWeek;

        for (var day = 1; day <= daysInMonth; day++)
        {
            var column = (startOffset + day - 1) % CalendarConstants.DaysPerWeek;
            var row = ((startOffset + day - 1) / CalendarConstants.DaysPerWeek) + 1;

            var hasEntries = false;
            var entryCount = 0;
            if (BingeCounts != null)
            {
                hasEntries = BingeCounts.TryGetValue(day, out entryCount);
            }

            Button dayButton = new()
            {
                Text = day.ToString(CultureInfo.InvariantCulture),
                CornerRadius = CalendarConstants.DayButtonCornerRadius,
                HeightRequest = CalendarConstants.DayButtonHeight,
                WidthRequest = CalendarConstants.DayButtonWidth,
                Padding = new Thickness(0),
                BackgroundColor = Color.FromArgb(CalendarConstants.NormalDayColor),
                TextColor = Color.FromArgb(CalendarConstants.NormalDayTextColor),
                FontAttributes = FontAttributes.Bold
            };

            if (hasEntries)
            {
                dayButton.BackgroundColor = Color.FromArgb(CalendarConstants.HasBingeEntryColor);
                dayButton.TextColor = Color.FromArgb(CalendarConstants.HasBingeEntryTextColor);
                SemanticProperties.SetDescription(dayButton,
                    string.Format(CultureInfo.CurrentCulture, "{0} entries on {1:MMMM} {2}",
                        entryCount,
                        Month,
                        day));
            }

            var capturedDay = day;
            dayButton.Clicked += (_, _) =>
            {
                var selectedDate = new DateTime(Month.Year, Month.Month, capturedDay);
                DayTapped?.Invoke(this, new DateSelectedEventArgs(selectedDate));
            };

            _calendarGrid.Add(dayButton, column, row);
        }
    }
}
