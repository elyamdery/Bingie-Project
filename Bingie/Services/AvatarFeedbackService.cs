using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Bingie.Models;

namespace Bingie.Services;

/// <summary>
/// Computes avatar scores, state transitions, and persists history.
/// </summary>
public sealed class AvatarFeedbackService
{
    private const string WeeklyPeriodType = "Week";
    private const string MonthlyPeriodType = "Month";

    private readonly IDataStore<BingeEntry> _bingeStore;
    private readonly IAvatarFeedbackRepository _repository;

    public AvatarFeedbackService(IDataStore<BingeEntry> bingeStore, IAvatarFeedbackRepository repository)
    {
        _bingeStore = bingeStore ?? throw new ArgumentNullException(nameof(bingeStore));
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public Task<AvatarFeedbackSettings> GetSettingsAsync(string username)
    {
        return GetOrCreateSettingsAsync(username);
    }

    public async Task UpdateHidePreferenceAsync(string username, bool hideAvatar)
    {
        var settings = await GetOrCreateSettingsAsync(username);
        if (settings.HideAvatar == hideAvatar) return;

        await _repository.UpsertSettingsAsync(settings.WithHideAvatar(hideAvatar));
    }

    public async Task<AvatarFeedbackSnapshot> GenerateSnapshotAsync(string username, DateTime nowUtc)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            throw new ArgumentException("Username cannot be empty.", nameof(username));
        }

        var normalizedUser = username.Trim();
        var entries = await LoadEntriesAsync(normalizedUser);
        var settings = await GetOrCreateSettingsAsync(normalizedUser);

        var today = nowUtc.Date;
        var weekStart = StartOfWeek(today);
        var weekEnd = weekStart.AddDays(7);
        var previousWeekStart = weekStart.AddDays(-7);

        var monthStart = new DateTime(today.Year, today.Month, 1, 0, 0, 0, DateTimeKind.Utc);
        var nextMonthStart = monthStart.AddMonths(1);
        var previousMonthStart = monthStart.AddMonths(-1);

        var weeklyPoints = SumWeighted(entries, weekStart, weekEnd);
        var previousWeeklyPoints = SumWeighted(entries, previousWeekStart, weekStart);

        var monthlyPoints = SumWeighted(entries, monthStart, nextMonthStart);
        var previousMonthlyPoints = SumWeighted(entries, previousMonthStart, monthStart);

        var weeklyScore = CalculateScore(weeklyPoints, settings.WeeklyGlowThreshold, settings.WeeklyConcernThreshold);
        var previousWeeklyScore = CalculateScore(previousWeeklyPoints, settings.WeeklyGlowThreshold, settings.WeeklyConcernThreshold);
        var weeklyDelta = Round(weeklyScore - previousWeeklyScore);

        var monthlyScore = CalculateScore(monthlyPoints, settings.MonthlyGlowThreshold, settings.MonthlyConcernThreshold);
        var previousMonthlyScore = CalculateScore(previousMonthlyPoints, settings.MonthlyGlowThreshold, settings.MonthlyConcernThreshold);
        var monthlyDelta = Round(monthlyScore - previousMonthlyScore);

        var currentBreakDays = BingeAnalytics.CurrentBreakDays(entries, nowUtc);
        var state = DetermineEnergyState(settings.HideAvatar, weeklyScore, weeklyDelta, monthlyDelta, currentBreakDays);
        var celebrate = !settings.HideAvatar && monthlyDelta >= 5;
        var animationKey = GetAnimationKey(state, celebrate);
        var copy = ComposeSupportiveCopy(state, currentBreakDays, weeklyDelta, monthlyDelta);

        var snapshot = new AvatarFeedbackSnapshot
        {
            EnergyState = state,
            WeeklyScore = weeklyScore,
            WeeklyDelta = weeklyDelta,
            MonthlyScore = monthlyScore,
            MonthlyDelta = monthlyDelta,
            IsHidden = settings.HideAvatar,
            AnimationKey = animationKey,
            SupportiveCopy = copy,
            GeneratedAtUtc = nowUtc,
            CurrentBreakDays = currentBreakDays,
            CelebrateMonthlyWin = celebrate
        };

        await PersistHistoryAsync(normalizedUser, snapshot, weekStart, monthStart);

        return snapshot;
    }

    private async Task<List<BingeEntry>> LoadEntriesAsync(string username)
    {
        IEnumerable<BingeEntry> items = await _bingeStore.GetItemsAsync();
        return items
            .Where(entry => string.Equals(entry.Username, username, StringComparison.OrdinalIgnoreCase))
            .OrderBy(entry => entry.Date)
            .ToList();
    }

    private async Task<AvatarFeedbackSettings> GetOrCreateSettingsAsync(string username)
    {
        var existing = await _repository.GetSettingsAsync(username.Trim());
        if (existing != null) return existing;

        var created = AvatarFeedbackSettings.CreateDefault(username);
        await _repository.UpsertSettingsAsync(created);
        return created;
    }

    private async Task PersistHistoryAsync(string username, AvatarFeedbackSnapshot snapshot, DateTime weekStart, DateTime monthStart)
    {
        if (!snapshot.IsHidden)
        {
            await PersistSnapshotIfNeededAsync(username, weekStart, WeeklyPeriodType, snapshot.WeeklyScore, snapshot.WeeklyDelta,
                snapshot.EnergyState, snapshot.SupportiveCopy);

            await PersistSnapshotIfNeededAsync(username, monthStart, MonthlyPeriodType, snapshot.MonthlyScore, snapshot.MonthlyDelta,
                snapshot.EnergyState, snapshot.SupportiveCopy);
        }
    }

    private async Task PersistSnapshotIfNeededAsync(
        string username,
        DateTime periodStart,
        string periodType,
        double score,
        double delta,
        AvatarEnergyState state,
        string supportiveCopy)
    {
        var existing = await _repository.GetSnapshotAsync(username, periodStart, periodType);
        if (existing != null) return;

        AvatarStateRecord record = new()
        {
            Username = username,
            PeriodStartUtc = periodStart,
            PeriodType = periodType,
            Score = score,
            EnergyState = state,
            DeltaFromPrevious = delta,
            SupportiveCopy = supportiveCopy,
            CreatedUtc = DateTime.UtcNow
        };

        await _repository.SaveSnapshotAsync(record);
    }

    private static DateTime StartOfWeek(DateTime date)
    {
        var offset = ((int)date.DayOfWeek + 6) % 7; // Monday as start
        var monday = date.AddDays(-offset);
        return new DateTime(monday.Year, monday.Month, monday.Day, 0, 0, 0, DateTimeKind.Utc);
    }

    private static double SumWeighted(IEnumerable<BingeEntry> entries, DateTime fromUtcInclusive, DateTime toUtcExclusive)
    {
        return entries
            .Where(entry =>
            {
                var date = Normalize(entry.Date);
                return date >= fromUtcInclusive && date < toUtcExclusive;
            })
            .Sum(GetSeverityWeight);
    }

    private static double GetSeverityWeight(BingeEntry entry)
    {
        var minutes = Math.Max(0, entry.Duration.TotalMinutes);

        return minutes switch
        {
            <= 0 => 1d,
            <= 15 => 1d,
            <= 45 => 1.3d,
            <= 120 => 1.6d,
            _ => 2d
        };
    }

    private static double CalculateScore(double weightedPoints, int glowThreshold, int concernThreshold)
    {
        if (glowThreshold < 0) glowThreshold = 0;
        if (concernThreshold <= glowThreshold) concernThreshold = glowThreshold + 1;

        if (weightedPoints <= glowThreshold)
        {
            var denominator = glowThreshold == 0 ? 1 : glowThreshold;
            var ratio = weightedPoints / denominator;
            return Round(100 - (ratio * 20));
        }

        if (weightedPoints >= concernThreshold)
        {
            var overflow = weightedPoints - concernThreshold;
            var penalty = Math.Min(20, overflow * 5);
            return Round(Math.Max(30, 45 - penalty));
        }

        var betweenRatio = (weightedPoints - glowThreshold) / (concernThreshold - glowThreshold);
        var interpolated = 80 - (betweenRatio * 35);
        return Round(interpolated);
    }

    private static AvatarEnergyState DetermineEnergyState(
        bool hideAvatar,
        double weeklyScore,
        double weeklyDelta,
        double monthlyDelta,
        int? breakDays)
    {
        if (hideAvatar) return AvatarEnergyState.Hidden;

        if (weeklyScore >= 80 || (weeklyDelta >= 5 && (breakDays ?? 0) >= 2))
        {
            return AvatarEnergyState.Energized;
        }

        if (weeklyScore >= 55 && weeklyDelta >= -15)
        {
            return AvatarEnergyState.Steady;
        }

        if (monthlyDelta >= 5)
        {
            return AvatarEnergyState.Steady;
        }

        return AvatarEnergyState.Tired;
    }

    private static string GetAnimationKey(AvatarEnergyState state, bool celebrate)
    {
        return state switch
        {
            AvatarEnergyState.Hidden => "avatar_hidden",
            AvatarEnergyState.Energized when celebrate => "avatar_energized_fireworks",
            AvatarEnergyState.Energized => "avatar_energized_glow",
            AvatarEnergyState.Steady => "avatar_steady_breathe",
            AvatarEnergyState.Tired => "avatar_tired_rest",
            _ => "avatar_steady_breathe"
        };
    }

    private static string ComposeSupportiveCopy(
        AvatarEnergyState state,
        int? breakDays,
        double weeklyDelta,
        double monthlyDelta)
    {
        return state switch
        {
            AvatarEnergyState.Hidden => "Avatar feedback is paused. Flip the switch whenever you want the glow back.",
            AvatarEnergyState.Energized when breakDays is >= 3 =>
                $"Glow-up! A {breakDays} day calm streak is fueling your avatar.",
            AvatarEnergyState.Energized =>
                "Your energy is rising. Those mindful check-ins are lighting things up.",
            AvatarEnergyState.Steady when monthlyDelta >= 0 =>
                "Steady glow. Gentle awareness is keeping things balanced.",
            AvatarEnergyState.Steady =>
                "Holding steady. Every log is a caring check-in.",
            AvatarEnergyState.Tired when weeklyDelta < 0 =>
                "This week dipped a bit. Offer yourself rest and compassion today.",
            AvatarEnergyState.Tired =>
                "Energy feels low. It's okay to slow down and reach out for support.",
            _ => "You're doing the work. Keep listening to your needs."
        };
    }

    private static double Round(double value) => Math.Round(value, 2, MidpointRounding.AwayFromZero);

    private static DateTime Normalize(DateTime value)
    {
        return value.Kind switch
        {
            DateTimeKind.Utc => value,
            DateTimeKind.Local => value.ToUniversalTime(),
            _ => DateTime.SpecifyKind(value, DateTimeKind.Utc)
        };
    }
}
