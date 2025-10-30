using System;

namespace Bingie.Models;

public sealed class CosmeticUnlockState
{
    public long Id { get; init; }
    public required string Username { get; init; }
    public required string CosmeticCode { get; init; }
    public required DateTime UnlockedUtc { get; init; }
    public bool Equipped { get; init; }

    public CosmeticUnlockState WithEquipped(bool equipped)
    {
        return new CosmeticUnlockState
        {
            Id = Id,
            Username = Username,
            CosmeticCode = CosmeticCode,
            UnlockedUtc = UnlockedUtc,
            Equipped = equipped
        };
    }
}
