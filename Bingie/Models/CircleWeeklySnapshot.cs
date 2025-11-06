using System;

namespace Bingie.Models;

/// <summary>
/// Stores the weekly aggregate metrics each member consents to share.
/// </summary>
public sealed class CircleWeeklySnapshot
{
    public int Id { get; set; }
    public int CircleId { get; set; }
    public required string Username { get; set; }
    public DateTime WeekStartUtc { get; set; }
    public int SharedXp { get; set; }
    public int SharedStreakDays { get; set; }
    public int SharedCopingCount { get; set; }
    public DateTime CreatedUtc { get; set; }
    public DateTime? RefreshedUtc { get; set; }
}
