namespace Bingie.Models;

public sealed class StoryChapterResult
{
    public required StoryTriggerDefinition Trigger { get; init; }
    public required string ChapterTitle { get; init; }
    public required string ChapterBody { get; init; }
    public StoryExperimentSuggestion? Experiment { get; init; }
}
