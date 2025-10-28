using System;
using System.Collections.Generic;
using System.Linq;
using Bingie.Models;

namespace Bingie.Services;

public static class BingeAnalytics
{
    public static int CountThisMonth(IEnumerable<BingeEntry> entries, DateTime todayUtc)
    {
        ArgumentNullException.ThrowIfNull(entries);
        var today = todayUtc.Date;
        var monthStart = new DateTime(today.Year, today.Month, 1);

        return entries.Count(entry =>
        {
            var local = entry.Date.ToLocalTime().Date;
            return local >= monthStart && local <= today;
        });
    }

    public static int? CurrentBreakDays(IEnumerable<BingeEntry> entries, DateTime todayUtc)
    {
        ArgumentNullException.ThrowIfNull(entries);
        var today = todayUtc.Date;
        var lastEntry = entries
            .OrderBy(entry => entry.Date)
            .LastOrDefault();

        if (lastEntry == null) return null;

        var lastDay = lastEntry.Date.ToLocalTime().Date;
        return (today - lastDay).Days;
    }

    public static IReadOnlyList<BingeDayStat> BuildTrend(IEnumerable<BingeEntry> entries, DateTime todayUtc,
        int movingAverageWindow = 7, int lookbackDays = 42)
    {
        ArgumentNullException.ThrowIfNull(entries);
        if (movingAverageWindow <= 0) throw new ArgumentOutOfRangeException(nameof(movingAverageWindow));
        if (lookbackDays < movingAverageWindow) lookbackDays = movingAverageWindow;

        var today = todayUtc.Date;
        var start = today.AddDays(-lookbackDays);

        Dictionary<DateTime, int> grouped = entries
            .GroupBy(entry => entry.Date.ToLocalTime().Date)
            .ToDictionary(group => group.Key, group => group.Count());

        List<BingeDayStat> stats = new();
        Queue<int> window = new();

        for (var cursor = start; cursor <= today; cursor = cursor.AddDays(1))
        {
            grouped.TryGetValue(cursor, out var count);

            if (window.Count == movingAverageWindow)
            {
                window.Dequeue();
            }

            window.Enqueue(count);
            var average = window.Count == 0 ? 0d : window.Average();

            stats.Add(new BingeDayStat(cursor, count, average));
        }

        return stats;
    }
}

