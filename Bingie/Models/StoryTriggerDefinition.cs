namespace Bingie.Models;

/// <summary>
/// Trigger metadata powering the story guide prompts.
/// </summary>
public sealed class StoryTriggerDefinition
{
    public required string Code { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required string ChapterIntro { get; init; }
}
