namespace Bingie.Models;

public sealed class CosmeticReward
{
    public required string CosmeticCode { get; init; }
    public required string Name { get; init; }
    public required string Description { get; init; }
    public int RequiredXp { get; init; }
}
