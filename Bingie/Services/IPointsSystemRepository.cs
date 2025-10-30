using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bingie.Models;

namespace Bingie.Services;

public interface IPointsSystemRepository
{
    Task<IReadOnlyList<PointActionDefinition>> GetActionDefinitionsAsync();
    Task<IReadOnlyList<CosmeticReward>> GetCosmeticRewardsAsync();

    Task<PointsSettings?> GetSettingsAsync(string username);
    Task UpsertSettingsAsync(PointsSettings settings);

    Task<IReadOnlyList<DailyQuestRecord>> GetDailyQuestsAsync(string username, DateTime dayStartUtc, DateTime dayEndUtc);
    Task<long> AddDailyQuestAsync(DailyQuestRecord quest);
    Task UpdateDailyQuestAsync(DailyQuestRecord quest);
    Task<DailyQuestRecord?> GetDailyQuestByIdAsync(long questId);

    Task AddXpTransactionAsync(XpTransaction transaction);
    Task<int> GetTotalXpAsync(string username);
    Task<IReadOnlyList<XpTransaction>> GetXpTransactionsAsync(string username, DateTime fromUtc, DateTime toUtc);
    Task<int> GetActionUsageCountAsync(string username, string actionCode, DateTime fromUtc, DateTime toUtc);

    Task<IReadOnlyList<CosmeticUnlockState>> GetCosmeticUnlockStatesAsync(string username);
    Task UpsertCosmeticUnlockStateAsync(CosmeticUnlockState state);
    Task ClearEquippedCosmeticsAsync(string username);
}
