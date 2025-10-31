namespace Bingie.Models;

public sealed class QuestViewModel
{
    public long QuestId { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
    public int Xp { get; init; }
    public bool Completed { get; init; }
}
