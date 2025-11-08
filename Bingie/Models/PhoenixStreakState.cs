using System;
using System.Collections.Generic;

namespace Bingie.Models;

public sealed class PhoenixStreakState
{
    public int CurrentStreakDays { get; init; }
    public bool RecoveryNeeded { get; init; }
    public bool GraceAvailable { get; init; }
    public int GraceTokens { get; init; }
    public DateTime GraceResetUtc { get; init; }
    public DateTime Today { get; init; }
    public int Threshold { get; init; }
    public IReadOnlyList<PhoenixStreakDay> History { get; init; } = Array.Empty<PhoenixStreakDay>();
}

public sealed class PhoenixStreakDay
{
    public required DateTime Date { get; init; }
    public int BingeCount { get; init; }
    public bool UnderThreshold { get; init; }
}
