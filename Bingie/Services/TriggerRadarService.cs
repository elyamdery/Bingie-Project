using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Bingie.Models;

namespace Bingie.Services;

public sealed class TriggerRadarService
{
    private static readonly (TimeSpan Start, TimeSpan End, string Label)[] TimeBuckets =
    {
        (TimeSpan.FromHours(0), TimeSpan.FromHours(4), "Late night"),
        (TimeSpan.FromHours(4), TimeSpan.FromHours(8), "Early morning"),
        (TimeSpan.FromHours(8), TimeSpan.FromHours(12), "Morning"),
        (TimeSpan.FromHours(12), TimeSpan.FromHours(16), "Afternoon"),
        (TimeSpan.FromHours(16), TimeSpan.FromHours(20), "Evening"),
        (TimeSpan.FromHours(20), TimeSpan.FromHours(24), "Night")
    };

    private readonly IDataStore<BingeEntry> _dataStore;

    public TriggerRadarService(IDataStore<BingeEntry> dataStore)
    {
        _dataStore = dataStore ?? throw new ArgumentNullException(nameof(dataStore));
    }

    public async Task<TriggerRadarResult?> GetAsync(string username, DateTime nowUtc, int lookbackDays = 42)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(username);
        var normalizedUser = username.Trim();
        var cutoff = nowUtc.ToUniversalTime().AddDays(-lookbackDays);

        IEnumerable<BingeEntry> entries = await _dataStore.GetItemsAsync();
        var recentEntries = entries
            .Where(e => string.Equals(e.Username, normalizedUser, StringComparison.OrdinalIgnoreCase))
            .Where(e => e.Date >= cutoff)
            .OrderBy(e => e.Date)
            .ToList();

        if (recentEntries.Count == 0)
        {
            return null;
        }

        var timeBuckets = BuildTimeBuckets(recentEntries, nowUtc);
        var dayBuckets = BuildDayBuckets(recentEntries, nowUtc);
        var summary = BuildSummary(timeBuckets, dayBuckets, recentEntries.Count);
        var suggestions = BuildSuggestions(timeBuckets, dayBuckets, nowUtc);

        return new TriggerRadarResult
        {
            TimeOfDayBuckets = timeBuckets,
            DayOfWeekBuckets = dayBuckets,
            Summary = summary,
            Suggestions = suggestions,
            TotalEvents = recentEntries.Count
        };
    }

    private static List<TriggerRadarBucket> BuildTimeBuckets(IReadOnlyCollection<BingeEntry> entries, DateTime nowUtc)
    {
        var buckets = new List<TriggerRadarBucket>(TimeBuckets.Length);
        var counts = new double[TimeBuckets.Length];
        var recentCounts = new double[TimeBuckets.Length];

        foreach (var entry in entries)
        {
            var local = entry.Date.ToLocalTime();
            var time = local.TimeOfDay;
            var index = Array.FindIndex(TimeBuckets, bucket => IsWithin(bucket, time));
            if (index < 0) index = TimeBuckets.Length - 1;
            counts[index]++;

            if (local >= nowUtc.AddDays(-14).ToLocalTime())
            {
                recentCounts[index]++;
            }
        }

        var max = counts.Max();
        if (max <= 0) max = 1;

        for (var i = 0; i < TimeBuckets.Length; i++)
        {
            var trend = CalculateTrend(entries, predicate: entry =>
            {
                var time = entry.Date.ToLocalTime().TimeOfDay;
                return IsWithin(TimeBuckets[i], time);
            }, nowUtc);

            buckets.Add(new TriggerRadarBucket
            {
                Label = TimeBuckets[i].Label,
                Intensity = counts[i] / max,
                Trend = trend
            });
        }

        return buckets;
    }

    private static List<TriggerRadarBucket> BuildDayBuckets(IReadOnlyCollection<BingeEntry> entries, DateTime nowUtc)
    {
        List<TriggerRadarBucket> buckets = new();
        var groups = entries.GroupBy(e => e.Date.ToLocalTime().DayOfWeek)
            .Select(g => (Day: g.Key, Count: g.Count()))
            .OrderBy(g => g.Day)
            .ToDictionary(g => g.Day, g => g.Count);

        var max = groups.Count == 0 ? 1 : groups.Max(g => g.Value);
        if (max <= 0) max = 1;

        foreach (DayOfWeek day in Enum.GetValues(typeof(DayOfWeek)))
        {
            groups.TryGetValue(day, out var count);
            var trend = CalculateTrend(entries, entry => entry.Date.ToLocalTime().DayOfWeek == day, nowUtc);
            buckets.Add(new TriggerRadarBucket
            {
                Label = day.ToString(),
                Intensity = count / (double)max,
                Trend = trend
            });
        }

        return buckets;
    }

    private static double CalculateTrend(IEnumerable<BingeEntry> entries, Func<BingeEntry, bool> predicate, DateTime nowUtc)
    {
        var recentStart = nowUtc.AddDays(-14);
        var previousStart = nowUtc.AddDays(-28);
        var recent = entries.Count(e => e.Date >= recentStart && predicate(e));
        var previous = entries.Count(e => e.Date >= previousStart && e.Date < recentStart && predicate(e));

        if (recent == previous) return 0;
        if (previous == 0) return recent > 0 ? 1 : 0;
        var change = (recent - previous) / (double)Math.Max(previous, 1);
        return Math.Max(-1, Math.Min(1, change));
    }

    private static bool IsWithin((TimeSpan Start, TimeSpan End, string Label) bucket, TimeSpan value)
    {
        if (bucket.Start <= value && value < bucket.End) return true;
        if (bucket.End == TimeSpan.FromHours(24) && value == TimeSpan.FromHours(24)) return true;
        return false;
    }

    private static string BuildSummary(IEnumerable<TriggerRadarBucket> timeBuckets, IEnumerable<TriggerRadarBucket> dayBuckets, int totalEvents)
    {
        var hottestTime = timeBuckets.OrderByDescending(b => b.Intensity).FirstOrDefault();
        var hottestDay = dayBuckets.OrderByDescending(b => b.Intensity).FirstOrDefault();

        if (hottestDay == null || hottestTime == null)
        {
            return $"Logged {totalEvents} check-ins the past few weeks. Keep noting when and why they occur.";
        }

        var timePhrase = hottestTime.Label.ToLower(CultureInfo.InvariantCulture);
        var dayPhrase = hottestDay.Label;
        return $"Most check-ins cluster around {timePhrase} on {dayPhrase}s ({totalEvents} recent events). Use this radar to plan gentle guardrails.";
    }

    private static List<string> BuildSuggestions(IReadOnlyList<TriggerRadarBucket> timeBuckets, IReadOnlyList<TriggerRadarBucket> dayBuckets, DateTime nowUtc)
    {
        List<string> suggestions = new();

        var topTimes = timeBuckets.OrderByDescending(b => b.Intensity).Take(2).ToList();
        var topDays = dayBuckets.OrderByDescending(b => b.Intensity).Take(2).ToList();

        foreach (var bucket in topTimes)
        {
            if (bucket.Intensity < 0.2) continue;
            var action = bucket.Trend > 0.1
                ? "Plan a grounding ritual"
                : "Keep reinforcing what works";
            suggestions.Add($"{action} for the {bucket.Label.ToLowerInvariant()} window.");
        }

        foreach (var day in topDays)
        {
            if (day.Intensity < 0.2) continue;
            var action = day.Trend > 0.1
                ? $"Prep extra support on {day.Label}s."
                : $"Celebrate steadier {day.Label}s—write down what helped.";
            suggestions.Add(action);
        }

        if (suggestions.Count == 0)
        {
            suggestions.Add("Patterns look balanced. Continue logging so we can spot new spikes early.");
        }

        return suggestions.Distinct().Take(4).ToList();
    }
}
