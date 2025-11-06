using System;

namespace Bingie.Models;

/// <summary>
/// Represents a positive encouragement sent between circle members.
/// </summary>
public sealed class SupportToken
{
    public int Id { get; set; }
    public int CircleId { get; set; }
    public required string FromUsername { get; set; }
    public required string ToUsername { get; set; }
    public required string TemplateCode { get; set; }
    public required string Message { get; set; }
    public DateTime WeekStartUtc { get; set; }
    public DateTime CreatedUtc { get; set; }
}
