using System;

namespace Bingie.Models;

public sealed class StoryExperimentSummary
{
    public required StoryExperimentDefinition Definition { get; init; }
    public StoryExperimentStatus Status { get; init; }
    public DateTime? LastSuggestedUtc { get; init; }
    public DateTime? PlannedUtc { get; init; }
    public DateTime? CompletedUtc { get; init; }
    public bool XpGranted { get; init; }
    public int XpReward { get; init; }
}
