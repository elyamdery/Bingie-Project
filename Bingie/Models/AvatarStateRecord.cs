using System;

namespace Bingie.Models;

/// <summary>
/// Persisted avatar state snapshot for weekly/monthly rollups.
/// </summary>
public sealed class AvatarStateRecord
{
    public long Id { get; init; }
    public required string Username { get; init; }
    public required DateTime PeriodStartUtc { get; init; }
    public required string PeriodType { get; init; } = string.Empty; // "Week" or "Month"
    public required double Score { get; init; }
    public required AvatarEnergyState EnergyState { get; init; }
    public required double DeltaFromPrevious { get; init; }
    public required string SupportiveCopy { get; init; }
    public required DateTime CreatedUtc { get; init; }
}
