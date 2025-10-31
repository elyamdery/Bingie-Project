using System;
using System.Collections.Generic;

namespace Bingie.Models;

public sealed class PointsDashboard
{
    public required DateTime SnapshotUtc { get; init; }
    public bool RewardsPaused { get; init; }
    public int TotalXp { get; init; }
    public int WeeklyXp { get; init; }
    public int GlowLevel { get; init; }
    public double GlowFill { get; init; }
    public required IReadOnlyList<QuestViewModel> Quests { get; init; }
    public required IReadOnlyList<CosmeticStatus> Cosmetics { get; init; }
    public required IReadOnlyList<DailyXpStat> WeeklyTrend { get; init; }
    public string? EquippedCosmeticCode { get; init; }
}
