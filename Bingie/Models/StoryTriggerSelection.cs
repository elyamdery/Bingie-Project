using System;

namespace Bingie.Models;

/// <summary>
/// Recorded trigger selections per binge entry.
/// </summary>
public sealed class StoryTriggerSelection
{
    public long Id { get; init; }
    public required string Username { get; init; }
    public required string TriggerCode { get; init; }
    public string? CustomTrigger { get; init; }
    public required DateTime EntryDateUtc { get; init; }
    public required DateTime CreatedUtc { get; init; }
}
