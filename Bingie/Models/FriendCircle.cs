using System;

namespace Bingie.Models;

/// <summary>
/// Represents a private accountability circle the user can join.
/// </summary>
public sealed class FriendCircle
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string InviteCode { get; set; }
    public DateTime CreatedUtc { get; set; }
}
