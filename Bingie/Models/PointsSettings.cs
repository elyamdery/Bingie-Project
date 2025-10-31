namespace Bingie.Models;

public sealed class PointsSettings
{
    public required string Username { get; init; }
    public bool RewardsPaused { get; init; }
    public string? EquippedCosmeticCode { get; init; }

    public PointsSettings WithPause(bool paused)
    {
        return new PointsSettings
        {
            Username = Username,
            RewardsPaused = paused,
            EquippedCosmeticCode = EquippedCosmeticCode
        };
    }

    public PointsSettings WithEquippedCosmetic(string? cosmeticCode)
    {
        return new PointsSettings
        {
            Username = Username,
            RewardsPaused = RewardsPaused,
            EquippedCosmeticCode = cosmeticCode
        };
    }
}
