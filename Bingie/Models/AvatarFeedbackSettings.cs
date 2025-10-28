using System;

namespace Bingie.Models;

/// <summary>
/// Per-user configuration that tunes avatar scoring thresholds and visibility.
/// </summary>
public sealed class AvatarFeedbackSettings
{
    public required string Username { get; init; }

    /// <summary>
    /// When true the avatar energy animations are fully suppressed.
    /// </summary>
    public bool HideAvatar { get; init; }

    /// <summary>
    /// Weighted binge count that still maps to a "glowing" weekly state.
    /// </summary>
    public int WeeklyGlowThreshold { get; init; }

    /// <summary>
    /// Weighted binge count that pushes the weekly state into recovery mode.
    /// </summary>
    public int WeeklyConcernThreshold { get; init; }

    /// <summary>
    /// Weighted binge count for the current month that still indicates an energized vibe.
    /// </summary>
    public int MonthlyGlowThreshold { get; init; }

    /// <summary>
    /// Weighted binge count for the month that signals a compassionate recovery nudge.
    /// </summary>
    public int MonthlyConcernThreshold { get; init; }

    public DateTime LastUpdatedUtc { get; init; }

    public static AvatarFeedbackSettings CreateDefault(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            throw new ArgumentException("Username cannot be empty.", nameof(username));
        }

        return new AvatarFeedbackSettings
        {
            Username = username.Trim(),
            HideAvatar = false,
            WeeklyGlowThreshold = 3,
            WeeklyConcernThreshold = 7,
            MonthlyGlowThreshold = 12,
            MonthlyConcernThreshold = 24,
            LastUpdatedUtc = DateTime.UtcNow
        };
    }

    public AvatarFeedbackSettings WithHideAvatar(bool hideAvatar)
    {
        return new AvatarFeedbackSettings
        {
            Username = Username,
            HideAvatar = hideAvatar,
            WeeklyGlowThreshold = WeeklyGlowThreshold,
            WeeklyConcernThreshold = WeeklyConcernThreshold,
            MonthlyGlowThreshold = MonthlyGlowThreshold,
            MonthlyConcernThreshold = MonthlyConcernThreshold,
            LastUpdatedUtc = DateTime.UtcNow
        };
    }
}
