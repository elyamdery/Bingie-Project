using System.Threading.Tasks;
using Bingie.Models;

namespace Bingie.Services;

public interface IAvatarFeedbackRepository
{
    Task<AvatarFeedbackSettings?> GetSettingsAsync(string username);
    Task UpsertSettingsAsync(AvatarFeedbackSettings settings);
    Task<AvatarStateRecord?> GetSnapshotAsync(string username, DateTime periodStartUtc, string periodType);
    Task<AvatarStateRecord?> GetLatestSnapshotAsync(string username, string periodType);
    Task SaveSnapshotAsync(AvatarStateRecord record);
}
