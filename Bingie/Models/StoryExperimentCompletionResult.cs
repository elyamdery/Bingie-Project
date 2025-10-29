namespace Bingie.Models;

public sealed class StoryExperimentCompletionResult
{
    public required StoryExperimentSummary Summary { get; init; }
    public int XpAwarded { get; init; }
}
