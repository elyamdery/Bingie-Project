using System;

namespace Bingie.Models;

public sealed class XpTransaction
{
    public long Id { get; init; }
    public required string Username { get; init; }
    public required string ActionCode { get; init; }
    public int Amount { get; init; }
    public required DateTime CreatedUtc { get; init; }
}
