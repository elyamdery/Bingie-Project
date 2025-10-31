namespace Bingie.Models;

public sealed class StoryExperimentSuggestion
{
    public required StoryExperimentDefinition Definition { get; init; }
    public bool AlreadyPlanned { get; init; }
}
