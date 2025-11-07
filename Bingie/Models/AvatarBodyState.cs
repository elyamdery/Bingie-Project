using System;

namespace Bingie.Models;

public sealed class AvatarBodyState
{
    public required DateTime Date { get; init; }
    public double Score { get; init; }
    public required string MoodCopy { get; init; }
}
