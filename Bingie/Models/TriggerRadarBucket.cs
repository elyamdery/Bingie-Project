namespace Bingie.Models;

public sealed class TriggerRadarBucket
{
    public required string Label { get; init; }
    public double Intensity { get; init; }
    public double Trend { get; init; }
}
