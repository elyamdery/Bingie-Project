using System;
using System.Collections.Generic;

namespace Bingie.Models;

public sealed class StoryWeeklyEpisode
{
    public required DateTime WeekStartUtc { get; init; }
    public required IReadOnlyList<StoryTriggerCount> TriggerCounts { get; init; }
    public required IReadOnlyList<StoryExperimentSummary> Experiments { get; init; }

    public bool HasData => TriggerCounts.Count > 0 || Experiments.Count > 0;
}
