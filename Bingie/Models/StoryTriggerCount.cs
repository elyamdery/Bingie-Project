namespace Bingie.Models;

public sealed class StoryTriggerCount
{
    public required StoryTriggerDefinition Definition { get; init; }
    public required int Count { get; init; }
}
