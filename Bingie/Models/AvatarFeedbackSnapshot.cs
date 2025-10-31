using System;

namespace Bingie.Models;

/// <summary>
/// Aggregated feedback response ready for binding in the UI.
/// </summary>
public sealed class AvatarFeedbackSnapshot
{
    public required AvatarEnergyState EnergyState { get; init; }
    public required double WeeklyScore { get; init; }
    public required double WeeklyDelta { get; init; }
    public required double MonthlyScore { get; init; }
    public required double MonthlyDelta { get; init; }
    public required bool IsHidden { get; init; }
    public required string AnimationKey { get; init; }
    public required string SupportiveCopy { get; init; }
    public required DateTime GeneratedAtUtc { get; init; }
    public int? CurrentBreakDays { get; init; }
    public bool CelebrateMonthlyWin { get; init; }
}
