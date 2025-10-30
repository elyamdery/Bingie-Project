namespace Bingie.Models;

public sealed class CosmeticStatus
{
    public required CosmeticReward Reward { get; init; }
    public bool IsUnlocked { get; init; }
    public bool IsEquipped { get; init; }
    public bool CanEquip { get; init; }
}
