using Bingie.Models;
using Bingie.Data;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Bingie.Views;

public class DailyCount
{
    public DateTime Date { get; set; }
    public int Count { get; set; }
}

public partial class StatisticsPage : ContentPage
{
    private readonly AppDBContext _context;
    private readonly string _username;
    private string _currentPeriod = "week";

    public StatisticsPage(AppDBContext context)
    {
        InitializeComponent();
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _username = "DefaultUser"; // Replace with actual logic to fetch username

        // Load statistics for the default period (week)
        LoadStatisticsAsync();
    }

    private async void LoadStatisticsAsync()
    {
        try
        {
            await UpdateStatistics();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Exception in LoadStatisticsAsync: {ex.Message}");
        }
    }    private async Task UpdateStatistics()
    {
        var (startDate, endDate) = GetDateRange(_currentPeriod);
        
        var records = await _context.BingeEntries
            .Include(b => b.User)
            .Where(b => b.User != null && 
                       b.User.Username == _username && 
                       b.Date >= startDate && 
                       b.Date <= endDate)
            .ToListAsync();

        var dailyGrouped = records
            .GroupBy(r => r.Date.Date)
            .Select(g => new DailyCount { Date = g.Key, Count = g.Count() })
            .OrderBy(x => x.Date)
            .ToList();

        // Calculate statistics
        var totalBinges = records.Count;
        var daysWithData = dailyGrouped.Count;
        var totalDays = (endDate - startDate).Days + 1;
        var dailyAverage = daysWithData > 0 ? Math.Round((double)totalBinges / totalDays, 1) : 0;
        
        var goodDays = dailyGrouped.Count(d => d.Count <= 5);
        var badDays = dailyGrouped.Count(d => d.Count > 10);
        
        var bestDay = dailyGrouped.Any() ? dailyGrouped.Min(d => d.Count) : 0;
        var worstDay = dailyGrouped.Any() ? dailyGrouped.Max(d => d.Count) : 0;

        // Calculate current streak
        var (streakDays, isGoodStreak) = CalculateCurrentStreak(dailyGrouped);

        // Update UI
        TotalBingesLabel.Text = totalBinges.ToString();
        DailyAverageLabel.Text = dailyAverage.ToString("F1");
        GoodDaysLabel.Text = goodDays.ToString();
        BadDaysLabel.Text = badDays.ToString();
        BestDayLabel.Text = bestDay.ToString();
        WorstDayLabel.Text = worstDay.ToString();
        
        CurrentStreakLabel.Text = $"{streakDays} days";
        StreakTypeLabel.Text = isGoodStreak ? "Current Good Streak (≤5 binges/day)" : "Days since last good day";
        
        // Update trend analysis
        UpdateTrendAnalysis(dailyGrouped);
        
        // Update time period label
        TimePeriodLabel.Text = $"Showing statistics for {GetPeriodDescription(_currentPeriod)}";
    }

    private (DateTime startDate, DateTime endDate) GetDateRange(string period)
    {
        var today = DateTime.Today;
        
        return period switch
        {
            "week" => (today.AddDays(-(int)today.DayOfWeek), today.AddDays(6 - (int)today.DayOfWeek)),
            "month" => (new DateTime(today.Year, today.Month, 1), new DateTime(today.Year, today.Month, DateTime.DaysInMonth(today.Year, today.Month))),
            "year" => (new DateTime(today.Year, 1, 1), new DateTime(today.Year, 12, 31)),
            "alltime" => (DateTime.MinValue, DateTime.MaxValue),
            _ => (today.AddDays(-7), today)
        };
    }

    private string GetPeriodDescription(string period)
    {
        return period switch
        {
            "week" => "this week",
            "month" => "this month", 
            "year" => "this year",
            "alltime" => "all time",
            _ => "this week"
        };
    }

    private (int streakDays, bool isGoodStreak) CalculateCurrentStreak(List<DailyCount> dailyData)
    {
        if (!dailyData.Any()) return (0, true);

        var today = DateTime.Today;
        var streakDays = 0;
        var isGoodStreak = true;

        // Check backwards from today
        for (var date = today; date >= dailyData.First().Date; date = date.AddDays(-1))
        {
            var dayData = dailyData.FirstOrDefault(d => d.Date == date);
            var bingeCount = dayData?.Count ?? 0;

            if (streakDays == 0)
            {
                // Determine if we're starting a good or bad streak
                isGoodStreak = bingeCount <= 5;
            }

            if ((isGoodStreak && bingeCount <= 5) || (!isGoodStreak && bingeCount > 5))
            {
                streakDays++;
            }
            else
            {
                break;
            }
        }

        return (streakDays, isGoodStreak);
    }

    private void UpdateTrendAnalysis(List<DailyCount> dailyData)
    {
        if (dailyData.Count < 2)
        {
            TrendLabel.Text = "Not enough data for trend analysis";
            return;
        }        var recentDays = dailyData.TakeLast(7).Select(d => d.Count).ToList();
        var earlierDays = dailyData.Count > 7 ? 
            dailyData.Skip(Math.Max(0, dailyData.Count - 14)).Take(7).Select(d => d.Count).ToList() : 
            new List<int>();

        if (!earlierDays.Any())
        {
            TrendLabel.Text = "Building trend data...";
            return;
        }

        var recentAvg = recentDays.Average();
        var earlierAvg = earlierDays.Average();
        
        var trend = recentAvg < earlierAvg ? "📈 Improving!" : 
                   recentAvg > earlierAvg ? "📉 Needs attention" : 
                   "➡️ Stable";
        
        var change = Math.Abs(recentAvg - earlierAvg);
        TrendLabel.Text = $"{trend} {change:F1} binges/day change vs previous period";
    }

    private void UpdateButtonStyles(string activePeriod)
    {
        // Reset all buttons
        WeekBtn.BackgroundColor = Color.FromRgb(149, 165, 166);
        MonthBtn.BackgroundColor = Color.FromRgb(149, 165, 166);
        YearBtn.BackgroundColor = Color.FromRgb(149, 165, 166);
        AllTimeBtn.BackgroundColor = Color.FromRgb(149, 165, 166);

        // Highlight active button
        var activeColor = Color.FromRgb(52, 152, 219);
        switch (activePeriod)
        {
            case "week": WeekBtn.BackgroundColor = activeColor; break;
            case "month": MonthBtn.BackgroundColor = activeColor; break;
            case "year": YearBtn.BackgroundColor = activeColor; break;
            case "alltime": AllTimeBtn.BackgroundColor = activeColor; break;
        }
    }

    private async void OnWeekClicked(object sender, EventArgs e)
    {
        _currentPeriod = "week";
        UpdateButtonStyles(_currentPeriod);
        await UpdateStatistics();
    }

    private async void OnMonthClicked(object sender, EventArgs e)
    {
        _currentPeriod = "month";
        UpdateButtonStyles(_currentPeriod);
        await UpdateStatistics();
    }

    private async void OnYearClicked(object sender, EventArgs e)
    {
        _currentPeriod = "year";
        UpdateButtonStyles(_currentPeriod);
        await UpdateStatistics();
    }    private async void OnAllTimeClicked(object sender, EventArgs e)
    {
        _currentPeriod = "alltime";
        UpdateButtonStyles(_currentPeriod);
        await UpdateStatistics();
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        try
        {
            Debug.WriteLine("StatisticsPage: Back button clicked");
            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Exception in OnBackClicked: {ex.Message}");
        }
    }
}