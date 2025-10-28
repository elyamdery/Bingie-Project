using System;

namespace Bingie.Models;

public sealed record BingeDayStat(DateTime Date, int Count, double MovingAverage);
