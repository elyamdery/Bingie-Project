namespace Bingie.Models;

/// <summary>
/// Defines a selectable encouragement template for support tokens.
/// </summary>
public sealed class SupportTokenTemplate
{
    public required string Code { get; init; }
    public required string Title { get; init; }
    public required string Body { get; init; }
}
