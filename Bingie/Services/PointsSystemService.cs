using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Bingie.Models;

namespace Bingie.Services;

public sealed class PointsSystemService
{
    private const int GlowMeterCycleXp = 100;

    private readonly IPointsSystemRepository _repository;

    private IReadOnlyList<PointActionDefinition>? _actionCache;
    private IReadOnlyDictionary<string, PointActionDefinition>? _actionLookup;
    private IReadOnlyList<CosmeticReward>? _cosmeticCache;
    private IReadOnlyDictionary<string, CosmeticReward>? _cosmeticLookup;

    public PointsSystemService(IPointsSystemRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<PointsDashboard> GetDashboardAsync(string username, DateTime snapshotUtc)
    {
        var normalizedUser = ValidateUser(username);
        var normalizedNow = NormalizeUtc(snapshotUtc);

        await EnsureCatalogAsync();
        var settings = await EnsureSettingsAsync(normalizedUser);

        var dayStart = normalizedNow.Date;
        var dayEnd = dayStart.AddDays(1);

        if (!settings.RewardsPaused)
        {
            await EnsureDailyQuestBoardAsync(normalizedUser, dayStart, normalizedNow);
        }

        var quests = await _repository.GetDailyQuestsAsync(normalizedUser, dayStart, dayEnd);
        var totalXp = await _repository.GetTotalXpAsync(normalizedUser);

        var weekStart = StartOfWeek(normalizedNow);
        var weekEnd = weekStart.AddDays(7);
        var weeklyTransactions = await _repository.GetXpTransactionsAsync(normalizedUser, weekStart, weekEnd);
        var weeklyXp = weeklyTransactions.Sum(tx => tx.Amount);

        var dailyStats = BuildDailyStats(weekStart, weeklyTransactions);

        var glowLevel = totalXp / GlowMeterCycleXp;
        var glowFill = (totalXp % GlowMeterCycleXp) / (double)GlowMeterCycleXp;

        var cosmeticStates = await EnsureCosmeticUnlocksAsync(normalizedUser, totalXp, normalizedNow);
        var cosmeticStatuses = BuildCosmeticStatuses(cosmeticStates, totalXp, settings.EquippedCosmeticCode);

        var questModels = quests
            .Select(q => MapQuest(q))
            .ToList();

        return new PointsDashboard
        {
            SnapshotUtc = normalizedNow,
            RewardsPaused = settings.RewardsPaused,
            TotalXp = totalXp,
            WeeklyXp = weeklyXp,
            GlowLevel = glowLevel,
            GlowFill = glowFill,
            Quests = questModels,
            Cosmetics = cosmeticStatuses,
            WeeklyTrend = dailyStats,
            EquippedCosmeticCode = settings.EquippedCosmeticCode
        };
    }

    public async Task CompleteQuestAsync(string username, long questId, DateTime nowUtc)
    {
        var normalizedUser = ValidateUser(username);
        var normalizedNow = NormalizeUtc(nowUtc);
        await EnsureCatalogAsync();

        var settings = await EnsureSettingsAsync(normalizedUser);
        if (settings.RewardsPaused)
        {
            throw new InvalidOperationException("Rewards are paused. Resume to complete quests.");
        }

        var quest = await _repository.GetDailyQuestByIdAsync(questId);
        if (quest == null)
        {
            throw new InvalidOperationException("Quest not found.");
        }

        if (!string.Equals(quest.Username, normalizedUser, StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException("Quest does not belong to the specified user.");
        }

        if (quest.CompletedUtc.HasValue)
        {
            return; // Already completed, nothing to do.
        }

        if (!_actionLookup!.TryGetValue(quest.ActionCode, out var action))
        {
            throw new InvalidOperationException("Quest action definition missing.");
        }

        await AwardXpAsync(normalizedUser, action, normalizedNow);

        var updatedQuest = quest.WithCompletion(normalizedNow, action.XpValue);
        await _repository.UpdateDailyQuestAsync(updatedQuest);
    }

    public async Task ToggleRewardsAsync(string username, bool paused, DateTime nowUtc)
    {
        var normalizedUser = ValidateUser(username);
        await EnsureCatalogAsync();

        var settings = await EnsureSettingsAsync(normalizedUser);
        if (settings.RewardsPaused == paused) return;

        await _repository.UpsertSettingsAsync(settings.WithPause(paused));
    }

    public async Task EquipCosmeticAsync(string username, string cosmeticCode, DateTime nowUtc)
    {
        var normalizedUser = ValidateUser(username);
        var normalizedNow = NormalizeUtc(nowUtc);
        await EnsureCatalogAsync();

        if (!_cosmeticLookup!.TryGetValue(cosmeticCode, out var reward))
        {
            throw new ArgumentOutOfRangeException(nameof(cosmeticCode), "Unknown cosmetic code.");
        }

        var totalXp = await _repository.GetTotalXpAsync(normalizedUser);
        if (totalXp < reward.RequiredXp)
        {
            throw new InvalidOperationException("Cosmetic not unlocked yet.");
        }

        var settings = await EnsureSettingsAsync(normalizedUser);
        var states = await _repository.GetCosmeticUnlockStatesAsync(normalizedUser);
        var state = states.FirstOrDefault(s => string.Equals(s.CosmeticCode, reward.CosmeticCode, StringComparison.OrdinalIgnoreCase));
        var unlockedUtc = state?.UnlockedUtc ?? normalizedNow;

        await _repository.ClearEquippedCosmeticsAsync(normalizedUser);

        CosmeticUnlockState updated = new()
        {
            Id = state?.Id ?? 0,
            Username = normalizedUser,
            CosmeticCode = reward.CosmeticCode,
            UnlockedUtc = unlockedUtc,
            Equipped = true
        };
        await _repository.UpsertCosmeticUnlockStateAsync(updated);

        await _repository.UpsertSettingsAsync(settings.WithEquippedCosmetic(reward.CosmeticCode));
    }

    public async Task RecordActionAsync(string username, string actionCode, DateTime nowUtc)
    {
        var normalizedUser = ValidateUser(username);
        var normalizedNow = NormalizeUtc(nowUtc);
        await EnsureCatalogAsync();

        if (!_actionLookup!.TryGetValue(actionCode, out var action)) return;

        var settings = await EnsureSettingsAsync(normalizedUser);
        if (settings.RewardsPaused) return;

        var dayStart = normalizedNow.Date;
        var dayEnd = dayStart.AddDays(1);
        var usageCount = await _repository.GetActionUsageCountAsync(normalizedUser, action.ActionCode, dayStart, dayEnd);
        if (usageCount >= action.DailyLimit) return;

        await AwardXpAsync(normalizedUser, action, normalizedNow);
    }

    private async Task EnsureCatalogAsync()
    {
        if (_actionCache == null)
        {
            var actions = await _repository.GetActionDefinitionsAsync();
            _actionCache = actions;
            _actionLookup = actions.ToDictionary(a => a.ActionCode, StringComparer.OrdinalIgnoreCase);
        }

        if (_cosmeticCache == null)
        {
            var cosmetics = await _repository.GetCosmeticRewardsAsync();
            _cosmeticCache = cosmetics;
            _cosmeticLookup = cosmetics.ToDictionary(c => c.CosmeticCode, StringComparer.OrdinalIgnoreCase);
        }
    }

    private async Task<PointsSettings> EnsureSettingsAsync(string username)
    {
        var settings = await _repository.GetSettingsAsync(username);
        if (settings != null) return settings;

        PointsSettings defaultSettings = new()
        {
            Username = username,
            RewardsPaused = false,
            EquippedCosmeticCode = null
        };

        await _repository.UpsertSettingsAsync(defaultSettings);
        return defaultSettings;
    }

    private async Task EnsureDailyQuestBoardAsync(string username, DateTime dayStartUtc, DateTime nowUtc)
    {
        var dayEnd = dayStartUtc.AddDays(1);
        var existing = await _repository.GetDailyQuestsAsync(username, dayStartUtc, dayEnd);
        if (existing.Count >= 3) return;

        var existingCodes = new HashSet<string>(existing.Select(q => q.ActionCode), StringComparer.OrdinalIgnoreCase);

        var eligibleActions = _actionCache!
            .Where(action => action.QuestEligible && !existingCodes.Contains(action.ActionCode))
            .ToList();

        if (eligibleActions.Count == 0) return;

        var seed = HashCode.Combine(username.ToLowerInvariant(), dayStartUtc.Year, dayStartUtc.DayOfYear);
        var random = new Random(seed);
        var desiredCount = Math.Clamp(3 + random.Next(0, 3), 3, Math.Min(5, eligibleActions.Count + existing.Count));

        var missingCount = Math.Max(0, desiredCount - existing.Count);
        if (missingCount == 0) return;

        var shuffled = eligibleActions
            .OrderBy(_ => random.Next())
            .Take(missingCount)
            .ToList();

        foreach (var action in shuffled)
        {
            DailyQuestRecord quest = new()
            {
                Id = 0,
                Username = username,
                ActionCode = action.ActionCode,
                QuestDateUtc = dayStartUtc,
                CreatedUtc = nowUtc,
                CompletedUtc = null,
                XpAwarded = 0
            };

            await _repository.AddDailyQuestAsync(quest);
        }
    }

    private async Task AwardXpAsync(string username, PointActionDefinition action, DateTime nowUtc)
    {
        XpTransaction transaction = new()
        {
            Id = 0,
            Username = username,
            ActionCode = action.ActionCode,
            Amount = action.XpValue,
            CreatedUtc = nowUtc
        };

        await _repository.AddXpTransactionAsync(transaction);
    }

    private async Task<IReadOnlyList<CosmeticUnlockState>> EnsureCosmeticUnlocksAsync(string username, int totalXp, DateTime nowUtc)
    {
        var existing = (await _repository.GetCosmeticUnlockStatesAsync(username)).ToList();
        var lookup = existing.ToDictionary(s => s.CosmeticCode, StringComparer.OrdinalIgnoreCase);

        foreach (var reward in _cosmeticCache!)
        {
            if (totalXp >= reward.RequiredXp && !lookup.ContainsKey(reward.CosmeticCode))
            {
                CosmeticUnlockState state = new()
                {
                    Id = 0,
                    Username = username,
                    CosmeticCode = reward.CosmeticCode,
                    UnlockedUtc = nowUtc,
                    Equipped = false
                };
                await _repository.UpsertCosmeticUnlockStateAsync(state);
                existing.Add(state);
                lookup[reward.CosmeticCode] = state;
            }
        }

        return existing;
    }

    private IReadOnlyList<CosmeticStatus> BuildCosmeticStatuses(IReadOnlyList<CosmeticUnlockState> states, int totalXp, string? equipped)
    {
        var stateLookup = states.ToDictionary(s => s.CosmeticCode, StringComparer.OrdinalIgnoreCase);
        List<CosmeticStatus> statuses = new();

        foreach (var reward in _cosmeticCache!)
        {
            stateLookup.TryGetValue(reward.CosmeticCode, out var state);
            var unlocked = totalXp >= reward.RequiredXp || state != null;
            var isEquipped = state?.Equipped ?? string.Equals(reward.CosmeticCode, equipped, StringComparison.OrdinalIgnoreCase);
            statuses.Add(new CosmeticStatus
            {
                Reward = reward,
                IsUnlocked = unlocked,
                IsEquipped = isEquipped,
                CanEquip = unlocked
            });
        }

        return statuses;
    }

    private static IReadOnlyList<DailyXpStat> BuildDailyStats(DateTime weekStart, IReadOnlyList<XpTransaction> transactions)
    {
        List<DailyXpStat> stats = new();
        for (var offset = 0; offset < 7; offset++)
        {
            var day = weekStart.AddDays(offset);
            var next = day.AddDays(1);
            var xp = transactions
                .Where(tx => tx.CreatedUtc >= day && tx.CreatedUtc < next)
                .Sum(tx => tx.Amount);

            stats.Add(new DailyXpStat
            {
                DateUtc = day,
                Xp = xp
            });
        }

        return stats;
    }

    private QuestViewModel MapQuest(DailyQuestRecord quest)
    {
        if (!_actionLookup!.TryGetValue(quest.ActionCode, out var action))
        {
            throw new InvalidOperationException("Action definition missing.");
        }

        return new QuestViewModel
        {
            QuestId = quest.Id,
            Title = action.Title,
            Description = action.Description,
            Xp = action.XpValue,
            Completed = quest.CompletedUtc.HasValue
        };
    }

    private static DateTime NormalizeUtc(DateTime value)
    {
        return value.Kind switch
        {
            DateTimeKind.Utc => value,
            DateTimeKind.Local => value.ToUniversalTime(),
            _ => DateTime.SpecifyKind(value, DateTimeKind.Utc)
        };
    }

    private static string ValidateUser(string username)
    {
        if (string.IsNullOrWhiteSpace(username)) throw new ArgumentException("Username cannot be empty.", nameof(username));
        return username.Trim();
    }

    private static DateTime StartOfWeek(DateTime dateUtc)
    {
        var mondayOffset = ((int)dateUtc.DayOfWeek + 6) % 7;
        var monday = dateUtc.Date.AddDays(-mondayOffset);
        return new DateTime(monday.Year, monday.Month, monday.Day, 0, 0, 0, DateTimeKind.Utc);
    }
}
