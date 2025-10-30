using System;

namespace Bingie.Models;

public sealed class DailyQuestRecord
{
    public long Id { get; init; }
    public required string Username { get; init; }
    public required string ActionCode { get; init; }
    public required DateTime QuestDateUtc { get; init; }
    public required DateTime CreatedUtc { get; init; }
    public DateTime? CompletedUtc { get; init; }
    public int XpAwarded { get; init; }

    public DailyQuestRecord WithCompletion(DateTime completedUtc, int xpAwarded)
    {
        return new DailyQuestRecord
        {
            Id = Id,
            Username = Username,
            ActionCode = ActionCode,
            QuestDateUtc = QuestDateUtc,
            CreatedUtc = CreatedUtc,
            CompletedUtc = completedUtc,
            XpAwarded = xpAwarded
        };
    }
}
