using System;

namespace Bingie.Models;

public sealed class DailyXpStat
{
    public required DateTime DateUtc { get; init; }
    public int Xp { get; init; }
}
