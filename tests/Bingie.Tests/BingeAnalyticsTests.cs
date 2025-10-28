using Bingie.Models;
using Bingie.Services;

namespace Bingie.Tests;

public class BingeAnalyticsTests
{
    [Fact]
    public void CountThisMonth_OnlyIncludesEntriesWithinCurrentMonth()
    {
        var today = new DateTime(2024, 7, 20, 0, 0, 0, DateTimeKind.Unspecified);
        var entries = new[]
        {
            new BingeEntry { Username = "alex", Date = new DateTime(2024, 7, 1, 9, 0, 0, DateTimeKind.Unspecified), Duration = TimeSpan.Zero },
            new BingeEntry { Username = "alex", Date = new DateTime(2024, 7, 19, 21, 0, 0, DateTimeKind.Unspecified), Duration = TimeSpan.Zero },
            new BingeEntry { Username = "alex", Date = new DateTime(2024, 6, 30, 12, 0, 0, DateTimeKind.Unspecified), Duration = TimeSpan.Zero }
        };

        var result = BingeAnalytics.CountThisMonth(entries, today);

        Assert.Equal(2, result);
    }

    [Fact]
    public void CurrentBreakDays_ReturnsNullWhenNoEntries()
    {
        var today = DateTime.Today;
        var breakDays = BingeAnalytics.CurrentBreakDays(Array.Empty<BingeEntry>(), today);

        Assert.Null(breakDays);
    }

    [Fact]
    public void CurrentBreakDays_ComputesDifferenceFromLatestEntry()
    {
        var today = new DateTime(2024, 7, 10, 0, 0, 0, DateTimeKind.Unspecified);
        var entries = new[]
        {
            new BingeEntry { Username = "alex", Date = new DateTime(2024, 7, 7, 12, 0, 0, DateTimeKind.Unspecified), Duration = TimeSpan.Zero },
            new BingeEntry { Username = "alex", Date = new DateTime(2024, 7, 8, 12, 0, 0, DateTimeKind.Unspecified), Duration = TimeSpan.Zero }
        };

        var breakDays = BingeAnalytics.CurrentBreakDays(entries, today);

        Assert.Equal(2, breakDays);
    }

    [Fact]
    public void BuildTrend_ReturnsWindowWithCountsAndMovingAverage()
    {
        var today = new DateTime(2024, 7, 10, 0, 0, 0, DateTimeKind.Unspecified);
        var entries = new[]
        {
            new BingeEntry { Username = "alex", Date = new DateTime(2024, 7, 8, 8, 0, 0, DateTimeKind.Unspecified), Duration = TimeSpan.Zero },
            new BingeEntry { Username = "alex", Date = new DateTime(2024, 7, 8, 18, 0, 0, DateTimeKind.Unspecified), Duration = TimeSpan.Zero },
            new BingeEntry { Username = "alex", Date = new DateTime(2024, 7, 9, 10, 0, 0, DateTimeKind.Unspecified), Duration = TimeSpan.Zero }
        };

        var stats = BingeAnalytics.BuildTrend(entries, today, movingAverageWindow: 3, lookbackDays: 3);

        Assert.Equal(4, stats.Count);

        var julyEighth = stats[^3]; // 8th of July
        Assert.Equal(2, julyEighth.Count);
        Assert.Equal(1, julyEighth.MovingAverage); // (0 + 2) / 2

        var julyNinth = stats[^2];
        Assert.Equal(1, julyNinth.Count);
        Assert.Equal((2 + 1) / 3d, julyNinth.MovingAverage);

        Assert.Equal(today.Date, stats.Last().Date);
        Assert.Equal(0, stats.Last().Count);
    }
}
