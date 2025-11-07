using System.Collections.Generic;

namespace Bingie.Models;

public sealed class TriggerRadarResult
{
    public IReadOnlyList<TriggerRadarBucket> TimeOfDayBuckets { get; init; } = new List<TriggerRadarBucket>();
    public IReadOnlyList<TriggerRadarBucket> DayOfWeekBuckets { get; init; } = new List<TriggerRadarBucket>();
    public required string Summary { get; init; }
    public IReadOnlyList<string> Suggestions { get; init; } = new List<string>();
    public int TotalEvents { get; init; }
}
