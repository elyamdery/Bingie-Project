using Bingie.Config;
using Bingie.Models;
using Bingie.Services;

namespace Bingie.Tests;

public class FriendsLeaderboardServiceTests
{
    private static readonly DateTime ReferenceWeek = new(2025, 3, 6, 10, 0, 0, DateTimeKind.Utc);

    [Fact]
    public async Task GetStateAsync_SortsEntriesAndHidesOptedOutMetrics()
    {
        using FeatureFlagScope scope = new();
        InMemoryFriendsRepository repository = new();
        FakePointsRepository points = new();
        InMemoryBingeStore bingeStore = new();
        FriendsLeaderboardService service = new(repository, points, bingeStore);

        await service.OptInAsync("alex", "Alex", shareXp: true, shareStreak: true, shareCopingCount: true, inviteCode: null, circleName: "Glow Squad", ReferenceWeek);

        var alexMembership = await repository.GetMembershipAsync("alex");
        Assert.NotNull(alexMembership);

        CircleMembership jamie = new()
        {
            CircleId = alexMembership!.CircleId,
            Username = "jamie",
            Nickname = "Jamie",
            ShareXp = true,
            ShareStreak = false,
            ShareCopingCount = false,
            Muted = false,
            JoinedUtc = ReferenceWeek,
            LastActiveUtc = ReferenceWeek
        };
        _ = await repository.AddMembershipAsync(jamie);

        points.AddTransaction("alex", "log_entry", 60, ReferenceWeek.AddDays(-1));
        points.AddTransaction("alex", "drink_water", 20, ReferenceWeek.AddDays(-1));
        bingeStore.ReplaceEntries("alex", new[]
        {
            new BingeEntry { Username = "alex", Date = ReferenceWeek.AddDays(-2), Duration = TimeSpan.Zero }
        });

        await repository.UpsertWeeklySnapshotAsync(new CircleWeeklySnapshot
        {
            CircleId = jamie.CircleId,
            Username = "jamie",
            WeekStartUtc = StartOfWeek(ReferenceWeek),
            SharedXp = 60,
            SharedStreakDays = 14,
            SharedCopingCount = 6,
            CreatedUtc = ReferenceWeek,
            RefreshedUtc = ReferenceWeek
        });

        var state = await service.GetStateAsync("alex", ReferenceWeek);

        Assert.True(state.OptedIn);
        Assert.Equal(2, state.Entries.Count);

        var leader = state.Entries[0];
        Assert.Equal("Alex", leader.Nickname);
        Assert.Equal(1, leader.Rank);
        Assert.Equal(80, leader.SharedXp);
        Assert.Equal(2, leader.SharedStreakDays);
        Assert.Equal(1, leader.SharedCopingCount);

        var runnerUp = state.Entries[1];
        Assert.Equal("Jamie", runnerUp.Nickname);
        Assert.Null(runnerUp.SharedStreakDays);
        Assert.Null(runnerUp.SharedCopingCount);
        Assert.Equal(60, runnerUp.SharedXp);
    }

    [Fact]
    public async Task SendSupportTokenAsync_OnlyAllowsCircleMembers()
    {
        using FeatureFlagScope scope = new();
        InMemoryFriendsRepository repository = new();
        FakePointsRepository points = new();
        InMemoryBingeStore bingeStore = new();
        FriendsLeaderboardService service = new(repository, points, bingeStore);

        await service.OptInAsync("alex", "Alex", true, true, true, null, "Glow Squad", ReferenceWeek);
        var membership = await repository.GetMembershipAsync("alex");
        Assert.NotNull(membership);

        CircleMembership kia = new()
        {
            CircleId = membership!.CircleId,
            Username = "kia",
            Nickname = "Kia",
            ShareXp = true,
            ShareStreak = true,
            ShareCopingCount = true,
            Muted = false,
            JoinedUtc = ReferenceWeek,
            LastActiveUtc = ReferenceWeek
        };
        _ = await repository.AddMembershipAsync(kia);

        await service.SendSupportTokenAsync("alex", "kia", "shine", ReferenceWeek);

        var tokens = await repository.GetSupportTokensForWeekAsync(kia.CircleId, StartOfWeek(ReferenceWeek));
        Assert.Single(tokens);
        Assert.Equal("alex", tokens[0].FromUsername);
        Assert.Equal("kia", tokens[0].ToUsername);
    }

    [Fact]
    public async Task OptInAsync_RespectsCapacityLimit()
    {
        using FeatureFlagScope scope = new();
        InMemoryFriendsRepository repository = new();
        FakePointsRepository points = new();
        InMemoryBingeStore bingeStore = new();
        FriendsLeaderboardService service = new(repository, points, bingeStore);

        await service.OptInAsync("host", "Host", true, true, true, null, "Circle", ReferenceWeek);
        var circle = await repository.GetMembershipAsync("host");
        Assert.NotNull(circle);

        for (var i = 0; i < FriendsLeaderboardService.CircleMemberCapacity - 1; i++)
        {
            CircleMembership extra = new()
            {
                CircleId = circle!.CircleId,
                Username = $"user{i}",
                Nickname = $"User {i}",
                ShareXp = true,
                ShareStreak = true,
                ShareCopingCount = true,
                Muted = false,
                JoinedUtc = ReferenceWeek,
                LastActiveUtc = ReferenceWeek
            };
            _ = await repository.AddMembershipAsync(extra);
        }

        var hostCircle = await repository.GetCircleByIdAsync(circle!.CircleId);
        Assert.NotNull(hostCircle);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            service.OptInAsync("latecomer", "Late", true, true, true, hostCircle!.InviteCode, null, ReferenceWeek));
    }

    [Fact]
    public async Task GetStateAsync_ReturnsOptOutWhenDisabled()
    {
        using FeatureFlagScope scope = new();
        FeatureFlags.OverrideLeaderboard(false);

        InMemoryFriendsRepository repository = new();
        FakePointsRepository points = new();
        InMemoryBingeStore bingeStore = new();
        FriendsLeaderboardService service = new(repository, points, bingeStore);

        var state = await service.GetStateAsync("alex", ReferenceWeek);

        Assert.False(state.OptedIn);
        Assert.True(state.Templates.Count > 0);
    }

    private static DateTime StartOfWeek(DateTime value)
    {
        var diff = (7 + (value.DayOfWeek - DayOfWeek.Monday)) % 7;
        return value.Date.AddDays(-diff);
    }

    private sealed class FeatureFlagScope : IDisposable
    {
        public FeatureFlagScope()
        {
            FeatureFlags.OverrideLeaderboard(true);
        }

        public void Dispose()
        {
            FeatureFlags.OverrideLeaderboard(null);
        }
    }

    private sealed class InMemoryFriendsRepository : IFriendsLeaderboardRepository
    {
        private readonly List<FriendCircle> _circles = new();
        private readonly List<CircleMembership> _memberships = new();
        private readonly List<CircleWeeklySnapshot> _snapshots = new();
        private readonly List<SupportToken> _tokens = new();
        private int _nextCircleId = 1;
        private int _nextMembershipId = 1;
        private int _nextTokenId = 1;

        public Task<FriendCircle?> GetCircleByInviteCodeAsync(string inviteCode)
        {
            var circle = _circles.FirstOrDefault(c =>
                string.Equals(c.InviteCode, inviteCode, StringComparison.OrdinalIgnoreCase));
            return Task.FromResult(circle);
        }

        public Task<FriendCircle?> GetCircleByIdAsync(int circleId)
        {
            return Task.FromResult(_circles.FirstOrDefault(c => c.Id == circleId));
        }

        public Task<int> AddCircleAsync(FriendCircle circle)
        {
            circle.Id = _nextCircleId++;
            _circles.Add(circle);
            return Task.FromResult(circle.Id);
        }

        public Task UpdateCircleAsync(FriendCircle circle)
        {
            return Task.CompletedTask;
        }

        public Task<CircleMembership?> GetMembershipAsync(string username)
        {
            var membership = _memberships.FirstOrDefault(m =>
                string.Equals(m.Username, username, StringComparison.OrdinalIgnoreCase));
            return Task.FromResult(membership);
        }

        public Task<CircleMembership?> GetMembershipByIdAsync(int membershipId)
        {
            return Task.FromResult(_memberships.FirstOrDefault(m => m.Id == membershipId));
        }

        public Task<IReadOnlyList<CircleMembership>> GetMembershipsForCircleAsync(int circleId)
        {
            IReadOnlyList<CircleMembership> members = _memberships.Where(m => m.CircleId == circleId).ToList();
            return Task.FromResult(members);
        }

        public Task<int> AddMembershipAsync(CircleMembership membership)
        {
            membership.Id = _nextMembershipId++;
            _memberships.Add(membership);
            return Task.FromResult(membership.Id);
        }

        public Task UpdateMembershipAsync(CircleMembership membership)
        {
            var index = _memberships.FindIndex(m => m.Id == membership.Id);
            if (index >= 0)
            {
                _memberships[index] = membership;
            }
            return Task.CompletedTask;
        }

        public Task RemoveMembershipAsync(int membershipId)
        {
            _memberships.RemoveAll(m => m.Id == membershipId);
            return Task.CompletedTask;
        }

        public Task<IReadOnlyList<CircleWeeklySnapshot>> GetSnapshotsForWeekAsync(int circleId, DateTime weekStartUtc)
        {
            IReadOnlyList<CircleWeeklySnapshot> snapshots = _snapshots
                .Where(s => s.CircleId == circleId && s.WeekStartUtc == weekStartUtc).ToList();
            return Task.FromResult(snapshots);
        }

        public Task UpsertWeeklySnapshotAsync(CircleWeeklySnapshot snapshot)
        {
            var existing = _snapshots.FirstOrDefault(s =>
                s.CircleId == snapshot.CircleId &&
                string.Equals(s.Username, snapshot.Username, StringComparison.OrdinalIgnoreCase) &&
                s.WeekStartUtc == snapshot.WeekStartUtc);
            if (existing != null)
            {
                existing.SharedXp = snapshot.SharedXp;
                existing.SharedStreakDays = snapshot.SharedStreakDays;
                existing.SharedCopingCount = snapshot.SharedCopingCount;
                existing.RefreshedUtc = snapshot.RefreshedUtc;
            }
            else
            {
                snapshot.Id = _snapshots.Count + 1;
                _snapshots.Add(snapshot);
            }

            return Task.CompletedTask;
        }

        public Task<IReadOnlyList<SupportToken>> GetSupportTokensForWeekAsync(int circleId, DateTime weekStartUtc)
        {
            IReadOnlyList<SupportToken> tokens = _tokens
                .Where(t => t.CircleId == circleId && t.WeekStartUtc == weekStartUtc)
                .OrderBy(t => t.CreatedUtc)
                .ToList();
            return Task.FromResult(tokens);
        }

        public Task<int> AddSupportTokenAsync(SupportToken token)
        {
            token.Id = _nextTokenId++;
            _tokens.Add(token);
            return Task.FromResult(token.Id);
        }
    }

    private sealed class FakePointsRepository : IPointsSystemRepository
    {
        private readonly Dictionary<string, List<XpTransaction>> _transactions = new(StringComparer.OrdinalIgnoreCase);

        public void AddTransaction(string username, string actionCode, int amount, DateTime createdUtc)
        {
            if (!_transactions.TryGetValue(username, out var list))
            {
                list = new List<XpTransaction>();
                _transactions[username] = list;
            }

            list.Add(new XpTransaction
            {
                Id = list.Count + 1,
                Username = username,
                ActionCode = actionCode,
                Amount = amount,
                CreatedUtc = createdUtc
            });
        }

        public Task<IReadOnlyList<XpTransaction>> GetXpTransactionsAsync(string username, DateTime fromUtc, DateTime toUtc)
        {
            if (!_transactions.TryGetValue(username, out var list))
            {
                return Task.FromResult<IReadOnlyList<XpTransaction>>(Array.Empty<XpTransaction>());
            }

            IReadOnlyList<XpTransaction> range = list
                .Where(tx => tx.CreatedUtc >= fromUtc && tx.CreatedUtc < toUtc)
                .ToList();
            return Task.FromResult(range);
        }

        #region Unused members
        public Task<IReadOnlyList<PointActionDefinition>> GetActionDefinitionsAsync() => throw new NotImplementedException();
        public Task<IReadOnlyList<CosmeticReward>> GetCosmeticRewardsAsync() => throw new NotImplementedException();
        public Task<PointsSettings?> GetSettingsAsync(string username) => throw new NotImplementedException();
        public Task UpsertSettingsAsync(PointsSettings settings) => throw new NotImplementedException();
        public Task<IReadOnlyList<DailyQuestRecord>> GetDailyQuestsAsync(string username, DateTime dayStartUtc, DateTime dayEndUtc) => throw new NotImplementedException();
        public Task<long> AddDailyQuestAsync(DailyQuestRecord quest) => throw new NotImplementedException();
        public Task UpdateDailyQuestAsync(DailyQuestRecord quest) => throw new NotImplementedException();
        public Task<DailyQuestRecord?> GetDailyQuestByIdAsync(long questId) => throw new NotImplementedException();
        public Task AddXpTransactionAsync(XpTransaction transaction) => throw new NotImplementedException();
        public Task<int> GetTotalXpAsync(string username) => throw new NotImplementedException();
        public Task<int> GetActionUsageCountAsync(string username, string actionCode, DateTime fromUtc, DateTime toUtc) => throw new NotImplementedException();
        public Task<IReadOnlyList<CosmeticUnlockState>> GetCosmeticUnlockStatesAsync(string username) => throw new NotImplementedException();
        public Task UpsertCosmeticUnlockStateAsync(CosmeticUnlockState state) => throw new NotImplementedException();
        public Task ClearEquippedCosmeticsAsync(string username) => throw new NotImplementedException();
        #endregion
    }

    private sealed class InMemoryBingeStore : IDataStore<BingeEntry>
    {
        private readonly List<BingeEntry> _entries = new();

        public void ReplaceEntries(string username, IEnumerable<BingeEntry> entries)
        {
            _entries.RemoveAll(e => string.Equals(e.Username, username, StringComparison.OrdinalIgnoreCase));
            _entries.AddRange(entries);
        }

        public Task<bool> AddItemAsync(BingeEntry item)
        {
            _entries.Add(item);
            return Task.FromResult(true);
        }

        public Task<bool> UpdateItemAsync(BingeEntry item) => Task.FromResult(true);
        public Task<bool> DeleteItemAsync(string id) => Task.FromResult(true);
        public Task<BingeEntry?> GetItemAsync(string id) => Task.FromResult<BingeEntry?>(null);

        public Task<IEnumerable<BingeEntry>> GetItemsAsync()
        {
            return Task.FromResult<IEnumerable<BingeEntry>>(_entries.ToList());
        }
    }
}
