namespace Bingie.Models;

/// <summary>
/// Describes a narrative experiment offered to the user.
/// </summary>
public sealed class StoryExperimentDefinition
{
    public required string Code { get; init; }
    public required string TriggerCode { get; init; }
    public required string Title { get; init; }
    public required string Prompt { get; init; }
    public int XpReward { get; init; } = 10;
}
