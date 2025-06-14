using Bingie.Data;
using Bingie.Models;
using Bingie.Services;
using System.Diagnostics;
using System.Globalization;
using Microsoft.Maui.Controls;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace Bingie.Views;

public partial class HistoryPage : ContentPage
{
    private readonly DbSet<BingeEntry> _bingeEntries;
    private readonly AppDBContext _dbContext;
    private readonly string _username;
    private DateTime _currentWeekStart;
    private readonly string[] _dayNames = { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };

    public HistoryPage() : this(IPlatformApplication.Current?.Services?.GetService(typeof(AppDBContext)) as AppDBContext ?? throw new InvalidOperationException("AppDBContext not available")) {}
    public HistoryPage(AppDBContext dbContext)
    {
        InitializeComponent();
        _dbContext = dbContext;
        _bingeEntries = _dbContext.BingeEntries;
        
        // Get current username from Preferences (set during login)
        _username = Preferences.Get("CurrentUsername", "DefaultUser");
        Debug.WriteLine($"HistoryPage: Using username: {_username}");
        
        // Set current week to start on Sunday
        var today = DateTime.Today;
        _currentWeekStart = today.AddDays(-(int)today.DayOfWeek);

        try
        {
            Debug.WriteLine("HistoryPage: Initializing Weekly View");
            InitializeWeeklyView();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Exception in HistoryPage constructor: {ex.Message}");
            throw;        }
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        try
        {
            Debug.WriteLine("HistoryPage: OnAppearing - Refreshing data");
            await UpdateWeeklyCalendar();
            await UpdateQuickStats();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Exception in OnAppearing: {ex.Message}");
        }
    }

    private async void InitializeWeeklyView()
    {
        CreateDayHeaders();
        await UpdateWeeklyCalendar();
        await UpdateQuickStats();
    }

    private void CreateDayHeaders()
    {
        DayHeadersGrid.Children.Clear();
        
        for (int i = 0; i < 7; i++)
        {
            var headerLabel = new Label
            {
                Text = _dayNames[i],
                FontSize = 14,
                FontAttributes = FontAttributes.Bold,
                TextColor = Colors.Gray,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };
            
            DayHeadersGrid.Add(headerLabel, i, 0);
        }
    }

    private async Task UpdateWeeklyCalendar()
    {
        try
        {
            Debug.WriteLine("HistoryPage: Updating weekly calendar");
            
            // Update header labels
            var weekNumber = GetWeekOfYear(_currentWeekStart);
            WeekLabel.Text = $"Week {weekNumber}";
            
            var weekEnd = _currentWeekStart.AddDays(6);
            WeekDateRange.Text = $"{_currentWeekStart:MMM dd} - {weekEnd:MMM dd}, {_currentWeekStart.Year}";

            WeekCalendarGrid.Children.Clear();            // Get binge data for the current week
            var weekStart = _currentWeekStart;
            var weekEndDate = _currentWeekStart.AddDays(7);
            
            Debug.WriteLine($"HistoryPage: Querying data for week {weekStart:yyyy-MM-dd} to {weekEndDate:yyyy-MM-dd}");
            Debug.WriteLine($"HistoryPage: Looking for username: {_username}");
            
            var records = await _bingeEntries
                .Include(b => b.User)
                .Where(b => b.User != null && 
                           b.User.Username == _username && 
                           b.Date >= weekStart && 
                           b.Date < weekEndDate)
                .ToListAsync();

            Debug.WriteLine($"HistoryPage: Found {records.Count} binge records for the week");
            foreach (var record in records)
            {
                Debug.WriteLine($"HistoryPage: Record - Date: {record.Date:yyyy-MM-dd}, User: {record.User?.Username}");
            }

            var dailyBingeCounts = records
                .GroupBy(r => r.Date.Date)
                .ToDictionary(g => g.Key, g => g.Count());

            // Create day buttons for the week
            for (int i = 0; i < 7; i++)
            {
                var currentDay = _currentWeekStart.AddDays(i);
                var bingeCount = dailyBingeCounts.GetValueOrDefault(currentDay.Date, 0);
                
                var dayFrame = CreateDayFrame(currentDay, bingeCount);
                WeekCalendarGrid.Add(dayFrame, i, 0);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Exception in UpdateWeeklyCalendar: {ex.Message}");
            throw;
        }
    }

    private Frame CreateDayFrame(DateTime date, int bingeCount)
    {
        var isToday = date.Date == DateTime.Today;
        var dayCategory = GetDayCategory(bingeCount);
        
        var dayFrame = new Frame
        {
            BackgroundColor = GetDayColor(dayCategory),
            CornerRadius = 12,
            HasShadow = isToday,
            Padding = 10,
            HeightRequest = 80,
            BorderColor = isToday ? Colors.Black : Colors.Transparent
        };

        var dayStack = new StackLayout
        {
            Spacing = 5,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center
        };

        var dayLabel = new Label
        {
            Text = date.Day.ToString(),
            FontSize = 16,
            FontAttributes = isToday ? FontAttributes.Bold : FontAttributes.None,
            TextColor = GetTextColor(dayCategory),
            HorizontalOptions = LayoutOptions.Center
        };

        var bingeLabel = new Label
        {
            Text = bingeCount.ToString(),
            FontSize = 20,
            FontAttributes = FontAttributes.Bold,
            TextColor = GetTextColor(dayCategory),
            HorizontalOptions = LayoutOptions.Center
        };

        var categoryLabel = new Label
        {
            Text = GetDayEmoji(dayCategory),
            FontSize = 12,
            HorizontalOptions = LayoutOptions.Center
        };

        dayStack.Children.Add(dayLabel);
        dayStack.Children.Add(bingeLabel);
        dayStack.Children.Add(categoryLabel);
        
        dayFrame.Content = dayStack;

        // Add tap gesture
        var tapGesture = new TapGestureRecognizer();
        tapGesture.Tapped += (s, e) => OnDaySelected(date);
        dayFrame.GestureRecognizers.Add(tapGesture);

        return dayFrame;
    }

    private string GetDayCategory(int bingeCount)
    {
        if (bingeCount <= 5) return "good";
        if (bingeCount <= 10) return "medium";
        return "bad";
    }

    private Color GetDayColor(string category)
    {
        return category switch
        {
            "good" => Color.FromRgb(232, 245, 232), // Light green
            "medium" => Color.FromRgb(255, 243, 224), // Light orange
            "bad" => Color.FromRgb(255, 235, 238), // Light red
            _ => Colors.LightGray
        };
    }

    private Color GetTextColor(string category)
    {
        return category switch
        {
            "good" => Color.FromRgb(39, 174, 96), // Green
            "medium" => Color.FromRgb(243, 156, 18), // Orange
            "bad" => Color.FromRgb(231, 76, 60), // Red
            _ => Colors.Gray
        };
    }

    private string GetDayEmoji(string category)
    {
        return category switch
        {
            "good" => "😊",
            "medium" => "😐",
            "bad" => "😞",
            _ => ""
        };
    }

    private async Task UpdateQuickStats()
    {
        try
        {
            var weekStart = _currentWeekStart;
            var weekEnd = _currentWeekStart.AddDays(7);
            
            var weekRecords = await _bingeEntries
                .Include(b => b.User)
                .Where(b => b.User != null && 
                           b.User.Username == _username && 
                           b.Date >= weekStart && 
                           b.Date < weekEnd)
                .ToListAsync();

            var dailyCounts = weekRecords
                .GroupBy(r => r.Date.Date)
                .Select(g => g.Count())
                .ToList();

            var weekTotal = weekRecords.Count;
            var weekAverage = dailyCounts.Any() ? Math.Round(dailyCounts.Average(), 1) : 0;
            var goodDays = dailyCounts.Count(c => c <= 5);
            var badDays = dailyCounts.Count(c => c > 10);

            WeekTotalLabel.Text = weekTotal.ToString();
            WeekAverageLabel.Text = weekAverage.ToString("F1");
            GoodDaysLabel.Text = goodDays.ToString();
            BadDaysLabel.Text = badDays.ToString();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Exception in UpdateQuickStats: {ex.Message}");
        }
    }

    private void OnDaySelected(DateTime selectedDate)
    {
        try
        {
            Debug.WriteLine($"HistoryPage: Day selected: {selectedDate:yyyy-MM-dd}");
            Navigation.PushAsync(new DayStatisticsPage(selectedDate, _dbContext));
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Exception in OnDaySelected: {ex.Message}");
        }
    }

    private async void OnPreviousWeekClicked(object sender, EventArgs e)
    {
        try
        {
            Debug.WriteLine("HistoryPage: Previous week clicked");
            _currentWeekStart = _currentWeekStart.AddDays(-7);
            await UpdateWeeklyCalendar();
            await UpdateQuickStats();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Exception in OnPreviousWeekClicked: {ex.Message}");
        }
    }

    private async void OnNextWeekClicked(object sender, EventArgs e)
    {
        try
        {
            Debug.WriteLine("HistoryPage: Next week clicked");
            _currentWeekStart = _currentWeekStart.AddDays(7);
            await UpdateWeeklyCalendar();
            await UpdateQuickStats();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Exception in OnNextWeekClicked: {ex.Message}");
        }
    }    private async void OnStatisticsClicked(object sender, EventArgs e)
    {
        try
        {
            Debug.WriteLine("HistoryPage: Statistics clicked");
            await Navigation.PushAsync(new StatisticsPage(_dbContext));
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Exception in OnStatisticsClicked: {ex.Message}");
        }
    }



    private int GetWeekOfYear(DateTime date)
    {
        var calendar = CultureInfo.CurrentCulture.Calendar;
        return calendar.GetWeekOfYear(date, CalendarWeekRule.FirstDay, DayOfWeek.Sunday);
    }
}