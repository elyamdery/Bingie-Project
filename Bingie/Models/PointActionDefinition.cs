namespace Bingie.Models;

public sealed class PointActionDefinition
{
    public required string ActionCode { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
    public int XpValue { get; init; }
    public int DailyLimit { get; init; }
    public bool QuestEligible { get; init; }
}
