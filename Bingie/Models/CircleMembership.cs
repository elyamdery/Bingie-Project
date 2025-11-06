using System;

namespace Bingie.Models;

/// <summary>
/// Records a user's participation and privacy preferences within a circle.
/// </summary>
public sealed class CircleMembership
{
    public int Id { get; set; }
    public int CircleId { get; set; }
    public required string Username { get; set; }
    public required string Nickname { get; set; }
    public bool ShareXp { get; set; }
    public bool ShareStreak { get; set; }
    public bool ShareCopingCount { get; set; }
    public bool Muted { get; set; }
    public DateTime JoinedUtc { get; set; }
    public DateTime? LastActiveUtc { get; set; }

    public CircleMembership WithSharing(bool shareXp, bool shareStreak, bool shareCopingCount)
    {
        return new CircleMembership
        {
            Id = Id,
            CircleId = CircleId,
            Username = Username,
            Nickname = Nickname,
            ShareXp = shareXp,
            ShareStreak = shareStreak,
            ShareCopingCount = shareCopingCount,
            Muted = Muted,
            JoinedUtc = JoinedUtc,
            LastActiveUtc = LastActiveUtc
        };
    }

    public CircleMembership WithMuteState(bool muted)
    {
        return new CircleMembership
        {
            Id = Id,
            CircleId = CircleId,
            Username = Username,
            Nickname = Nickname,
            ShareXp = ShareXp,
            ShareStreak = ShareStreak,
            ShareCopingCount = ShareCopingCount,
            Muted = muted,
            JoinedUtc = JoinedUtc,
            LastActiveUtc = LastActiveUtc
        };
    }

    public CircleMembership WithNickname(string nickname)
    {
        return new CircleMembership
        {
            Id = Id,
            CircleId = CircleId,
            Username = Username,
            Nickname = nickname,
            ShareXp = ShareXp,
            ShareStreak = ShareStreak,
            ShareCopingCount = ShareCopingCount,
            Muted = Muted,
            JoinedUtc = JoinedUtc,
            LastActiveUtc = LastActiveUtc
        };
    }
}
