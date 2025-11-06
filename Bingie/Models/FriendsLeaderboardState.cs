using System;
using System.Collections.Generic;

namespace Bingie.Models;

/// <summary>
/// Snapshot of the leaderboard, membership, and support activity for the current week.
/// </summary>
public sealed class FriendsLeaderboardState
{
    public bool OptedIn { get; init; }
    public DateTime WeekStartUtc { get; init; }
    public CircleSummary? Circle { get; init; }
    public CircleMembership? Membership { get; init; }
    public IReadOnlyList<LeaderboardEntry> Entries { get; init; } = Array.Empty<LeaderboardEntry>();
    public IReadOnlyList<SupportToken> SupportTokens { get; init; } = Array.Empty<SupportToken>();
    public IReadOnlyList<SupportTokenTemplate> Templates { get; init; } = Array.Empty<SupportTokenTemplate>();
}

/// <summary>
/// Lightweight descriptor for the circle presented to the user.
/// </summary>
public sealed class CircleSummary
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required string InviteCode { get; init; }
    public int MemberCount { get; init; }
    public int MemberCapacity { get; init; }
}

/// <summary>
/// Leaderboard row prepared for UI consumption.
/// </summary>
public sealed class LeaderboardEntry
{
    public required string Username { get; init; }
    public required string Nickname { get; init; }
    public int Rank { get; init; }
    public bool IsSelf { get; init; }
    public bool Muted { get; init; }
    public int SupportTokensReceived { get; init; }
    public int? SharedXp { get; init; }
    public int? SharedStreakDays { get; init; }
    public int? SharedCopingCount { get; init; }
    public bool SharesXp => SharedXp.HasValue;
    public bool SharesStreak => SharedStreakDays.HasValue;
    public bool SharesCoping => SharedCopingCount.HasValue;
    public double CompositeScore { get; init; }
}
