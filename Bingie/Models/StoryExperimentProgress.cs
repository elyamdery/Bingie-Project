using System;

namespace Bingie.Models;

/// <summary>
/// Tracks the lifecycle of an experiment per user.
/// </summary>
public sealed class StoryExperimentProgress
{
    public long Id { get; init; }
    public required string Username { get; init; }
    public required string ExperimentCode { get; init; }
    public StoryExperimentStatus Status { get; init; }
    public DateTime? LastSuggestedUtc { get; init; }
    public DateTime? PlannedUtc { get; init; }
    public DateTime? CompletedUtc { get; init; }
    public bool XpGranted { get; init; }

    public StoryExperimentProgress With(
        StoryExperimentStatus? status = null,
        DateTime? lastSuggestedUtc = null,
        DateTime? plannedUtc = null,
        DateTime? completedUtc = null,
        bool? xpGranted = null)
    {
        return new StoryExperimentProgress
        {
            Id = Id,
            Username = Username,
            ExperimentCode = ExperimentCode,
            Status = status ?? Status,
            LastSuggestedUtc = lastSuggestedUtc ?? LastSuggestedUtc,
            PlannedUtc = plannedUtc ?? PlannedUtc,
            CompletedUtc = completedUtc ?? CompletedUtc,
            XpGranted = xpGranted ?? XpGranted
        };
    }
}
