using System.Linq;
using Bingie.Models;
using Bingie.Services;

namespace Bingie.Tests;

public class PointsSystemServiceTests
{
    private static readonly DateTime ReferenceDate = new(2025, 3, 1, 12, 0, 0, DateTimeKind.Utc);

    [Fact]
    public async Task GetDashboardAsync_GeneratesQuestBoard()
    {
        InMemoryPointsRepository repo = new();
        PointsSystemService service = new(repo);

        var dashboard = await service.GetDashboardAsync("alex", ReferenceDate);

        Assert.True(dashboard.Quests.Count >= 3);
        Assert.False(dashboard.RewardsPaused);
    }

    [Fact]
    public async Task CompleteQuestAsync_AwardsXpOnlyOnce()
    {
        InMemoryPointsRepository repo = new();
        PointsSystemService service = new(repo);

        var dashboard = await service.GetDashboardAsync("alex", ReferenceDate);
        var quest = dashboard.Quests.First();

        await service.CompleteQuestAsync("alex", quest.QuestId, ReferenceDate.AddMinutes(30));
        var totalAfterFirst = await repo.GetTotalXpAsync("alex");
        Assert.True(totalAfterFirst > 0);

        await service.CompleteQuestAsync("alex", quest.QuestId, ReferenceDate.AddMinutes(60));
        var totalAfterSecond = await repo.GetTotalXpAsync("alex");
        Assert.Equal(totalAfterFirst, totalAfterSecond);
    }

    [Fact]
    public async Task RecordActionAsync_HonorsDailyLimit()
    {
        InMemoryPointsRepository repo = new();
        PointsSystemService service = new(repo);

        await service.RecordActionAsync("alex", "log_entry", ReferenceDate);
        await service.RecordActionAsync("alex", "log_entry", ReferenceDate.AddMinutes(10));
        await service.RecordActionAsync("alex", "log_entry", ReferenceDate.AddMinutes(20));

        var totalXp = await repo.GetTotalXpAsync("alex");
        // log_entry yields 30 XP with daily limit 3; third call should be ignored
        Assert.Equal(60, totalXp);
    }

    [Fact]
    public async Task ToggleRewardsAsync_DisablesQuestCompletion()
    {
        InMemoryPointsRepository repo = new();
        PointsSystemService service = new(repo);

        var initialDashboard = await service.GetDashboardAsync("alex", ReferenceDate);
        var questId = initialDashboard.Quests.First().QuestId;

        await service.ToggleRewardsAsync("alex", paused: true, ReferenceDate);
        var pausedDashboard = await service.GetDashboardAsync("alex", ReferenceDate);
        Assert.True(pausedDashboard.RewardsPaused);

        await Assert.ThrowsAsync<InvalidOperationException>(() => service.CompleteQuestAsync("alex", questId, ReferenceDate.AddMinutes(30)));
    }

    private sealed class InMemoryPointsRepository : IPointsSystemRepository
    {
        private readonly List<PointActionDefinition> _actions;
        private readonly List<CosmeticReward> _cosmetics;
        private readonly Dictionary<string, PointsSettings> _settings = new(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<long, DailyQuestRecord> _quests = new();
        private readonly Dictionary<string, List<DailyQuestRecord>> _questsByUser = new(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, List<XpTransaction>> _transactions = new(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, List<CosmeticUnlockState>> _cosmeticsByUser = new(StringComparer.OrdinalIgnoreCase);
        private long _nextQuestId = 1;
        private long _nextTransactionId;

        public InMemoryPointsRepository()
        {
            _actions = new List<PointActionDefinition>
            {
                new PointActionDefinition { ActionCode = "log_entry", Title = "Log a check-in", Description = "Record an entry.", XpValue = 30, DailyLimit = 2, QuestEligible = true },
                new PointActionDefinition { ActionCode = "drink_water", Title = "Hydration pause", Description = "Drink a glass of water.", XpValue = 20, DailyLimit = 2, QuestEligible = true },
                new PointActionDefinition { ActionCode = "reflect_feelings", Title = "Name your feeling", Description = "Reflect with one sentence.", XpValue = 25, DailyLimit = 1, QuestEligible = true },
                new PointActionDefinition { ActionCode = "kind_note", Title = "Leave a kind note", Description = "Send a kind message.", XpValue = 15, DailyLimit = 1, QuestEligible = true }
            };

            _cosmetics = new List<CosmeticReward>
            {
                new CosmeticReward { CosmeticCode = "glow_ring", Name = "Glow Ring", Description = "Glow ring", RequiredXp = 100 }
            };
        }

        public Task<IReadOnlyList<PointActionDefinition>> GetActionDefinitionsAsync()
        {
            return Task.FromResult<IReadOnlyList<PointActionDefinition>>(_actions);
        }

        public Task<IReadOnlyList<CosmeticReward>> GetCosmeticRewardsAsync()
        {
            return Task.FromResult<IReadOnlyList<CosmeticReward>>(_cosmetics);
        }

        public Task<PointsSettings?> GetSettingsAsync(string username)
        {
            _settings.TryGetValue(username, out var settings);
            return Task.FromResult(settings);
        }

        public Task UpsertSettingsAsync(PointsSettings settings)
        {
            _settings[settings.Username] = settings;
            return Task.CompletedTask;
        }

        public Task<IReadOnlyList<DailyQuestRecord>> GetDailyQuestsAsync(string username, DateTime dayStartUtc, DateTime dayEndUtc)
        {
            if (!_questsByUser.TryGetValue(username, out var list))
            {
                return Task.FromResult<IReadOnlyList<DailyQuestRecord>>(Array.Empty<DailyQuestRecord>());
            }

            return Task.FromResult<IReadOnlyList<DailyQuestRecord>>(list
                .Where(q => q.QuestDateUtc >= dayStartUtc && q.QuestDateUtc < dayEndUtc)
                .Select(CloneQuest)
                .ToList());
        }

        public Task<long> AddDailyQuestAsync(DailyQuestRecord quest)
        {
            var clone = CloneQuest(quest);
            clone = new DailyQuestRecord
            {
                Id = _nextQuestId++,
                Username = clone.Username,
                ActionCode = clone.ActionCode,
                QuestDateUtc = clone.QuestDateUtc,
                CreatedUtc = clone.CreatedUtc,
                CompletedUtc = clone.CompletedUtc,
                XpAwarded = clone.XpAwarded
            };

            _quests[clone.Id] = clone;
            if (!_questsByUser.TryGetValue(clone.Username, out var list))
            {
                list = new List<DailyQuestRecord>();
                _questsByUser[clone.Username] = list;
            }
            list.Add(clone);
            return Task.FromResult(clone.Id);
        }

        public Task UpdateDailyQuestAsync(DailyQuestRecord quest)
        {
            if (!_quests.TryGetValue(quest.Id, out var existing)) return Task.CompletedTask;

            existing = quest;
            _quests[quest.Id] = existing;

            if (_questsByUser.TryGetValue(quest.Username, out var list))
            {
                var index = list.FindIndex(q => q.Id == quest.Id);
                if (index >= 0) list[index] = quest;
            }

            return Task.CompletedTask;
        }

        public Task<DailyQuestRecord?> GetDailyQuestByIdAsync(long questId)
        {
            _quests.TryGetValue(questId, out var quest);
            return Task.FromResult(quest);
        }

        public Task AddXpTransactionAsync(XpTransaction transaction)
        {
            var clone = new XpTransaction
            {
                Id = ++_nextTransactionId,
                Username = transaction.Username,
                ActionCode = transaction.ActionCode,
                Amount = transaction.Amount,
                CreatedUtc = transaction.CreatedUtc
            };

            if (!_transactions.TryGetValue(clone.Username, out var list))
            {
                list = new List<XpTransaction>();
                _transactions[clone.Username] = list;
            }

            list.Add(clone);
            return Task.CompletedTask;
        }

        public Task<int> GetTotalXpAsync(string username)
        {
            if (!_transactions.TryGetValue(username, out var list)) return Task.FromResult(0);
            return Task.FromResult(list.Sum(tx => tx.Amount));
        }

        public Task<IReadOnlyList<XpTransaction>> GetXpTransactionsAsync(string username, DateTime fromUtc, DateTime toUtc)
        {
            if (!_transactions.TryGetValue(username, out var list))
            {
                return Task.FromResult<IReadOnlyList<XpTransaction>>(Array.Empty<XpTransaction>());
            }

            return Task.FromResult<IReadOnlyList<XpTransaction>>(list
                .Where(tx => tx.CreatedUtc >= fromUtc && tx.CreatedUtc < toUtc)
                .Select(tx => new XpTransaction
                {
                    Id = tx.Id,
                    Username = tx.Username,
                    ActionCode = tx.ActionCode,
                    Amount = tx.Amount,
                    CreatedUtc = tx.CreatedUtc
                })
                .ToList());
        }

        public Task<int> GetActionUsageCountAsync(string username, string actionCode, DateTime fromUtc, DateTime toUtc)
        {
            if (!_transactions.TryGetValue(username, out var list)) return Task.FromResult(0);
            return Task.FromResult(list.Count(tx => tx.ActionCode == actionCode && tx.CreatedUtc >= fromUtc && tx.CreatedUtc < toUtc));
        }

        public Task<IReadOnlyList<CosmeticUnlockState>> GetCosmeticUnlockStatesAsync(string username)
        {
            if (!_cosmeticsByUser.TryGetValue(username, out var list))
            {
                return Task.FromResult<IReadOnlyList<CosmeticUnlockState>>(Array.Empty<CosmeticUnlockState>());
            }

            return Task.FromResult<IReadOnlyList<CosmeticUnlockState>>(list.Select(CloneCosmetic).ToList());
        }

        public Task UpsertCosmeticUnlockStateAsync(CosmeticUnlockState state)
        {
            if (!_cosmeticsByUser.TryGetValue(state.Username, out var list))
            {
                list = new List<CosmeticUnlockState>();
                _cosmeticsByUser[state.Username] = list;
            }

            var index = list.FindIndex(c => string.Equals(c.CosmeticCode, state.CosmeticCode, StringComparison.OrdinalIgnoreCase));
            if (index >= 0)
            {
                list[index] = CloneCosmetic(state);
            }
            else
            {
                list.Add(CloneCosmetic(state));
            }

            return Task.CompletedTask;
        }

        public Task ClearEquippedCosmeticsAsync(string username)
        {
            if (_cosmeticsByUser.TryGetValue(username, out var list))
            {
                for (var i = 0; i < list.Count; i++)
                {
                    var current = list[i];
                    list[i] = current.WithEquipped(false);
                }
            }

            return Task.CompletedTask;
        }

        private static DailyQuestRecord CloneQuest(DailyQuestRecord quest)
        {
            return new DailyQuestRecord
            {
                Id = quest.Id,
                Username = quest.Username,
                ActionCode = quest.ActionCode,
                QuestDateUtc = quest.QuestDateUtc,
                CreatedUtc = quest.CreatedUtc,
                CompletedUtc = quest.CompletedUtc,
                XpAwarded = quest.XpAwarded
            };
        }

        private static CosmeticUnlockState CloneCosmetic(CosmeticUnlockState state)
        {
            return new CosmeticUnlockState
            {
                Id = state.Id,
                Username = state.Username,
                CosmeticCode = state.CosmeticCode,
                UnlockedUtc = state.UnlockedUtc,
                Equipped = state.Equipped
            };
        }
    }
}
