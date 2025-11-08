namespace Bingie.Models;

public sealed class PhoenixStreakSettings
{
    public required string Username { get; init; }
    public int DailyBingeThreshold { get; init; }
    public bool HideStreak { get; init; }
    public DateTime LastUpdatedUtc { get; init; }
    public DateTime LastGraceResetUtc { get; init; }
    public int GraceTokens { get; init; }

    public static PhoenixStreakSettings CreateDefault(string username, DateTime nowUtc)
    {
        return new PhoenixStreakSettings
        {
            Username = username,
            DailyBingeThreshold = 0,
            HideStreak = false,
            LastUpdatedUtc = nowUtc,
            LastGraceResetUtc = StartOfWeek(nowUtc),
            GraceTokens = 1
        };
    }

    public PhoenixStreakSettings WithThreshold(int threshold, DateTime nowUtc)
    {
        return new PhoenixStreakSettings
        {
            Username = Username,
            DailyBingeThreshold = Math.Max(0, threshold),
            HideStreak = HideStreak,
            LastUpdatedUtc = nowUtc,
            LastGraceResetUtc = LastGraceResetUtc,
            GraceTokens = GraceTokens
        };
    }

    public PhoenixStreakSettings WithHide(bool hide, DateTime nowUtc)
    {
        return new PhoenixStreakSettings
        {
            Username = Username,
            DailyBingeThreshold = DailyBingeThreshold,
            HideStreak = hide,
            LastUpdatedUtc = nowUtc,
            LastGraceResetUtc = LastGraceResetUtc,
            GraceTokens = GraceTokens
        };
    }

    public PhoenixStreakSettings WithGrace(int tokens, DateTime resetUtc)
    {
        return new PhoenixStreakSettings
        {
            Username = Username,
            DailyBingeThreshold = DailyBingeThreshold,
            HideStreak = HideStreak,
            LastUpdatedUtc = LastUpdatedUtc,
            LastGraceResetUtc = resetUtc,
            GraceTokens = tokens
        };
    }

    private static DateTime StartOfWeek(DateTime date)
    {
        var offset = ((int)date.DayOfWeek + 6) % 7;
        return date.Date.AddDays(-offset);
    }
}
