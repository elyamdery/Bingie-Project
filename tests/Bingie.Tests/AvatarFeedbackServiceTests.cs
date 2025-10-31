using Bingie.Models;
using Bingie.Services;
using Bingie.Tests.TestDoubles;

namespace Bingie.Tests;

public class AvatarFeedbackServiceTests
{
    private static readonly DateTime ReferenceDate = new(2025, 2, 15, 12, 0, 0, DateTimeKind.Utc);

    [Fact]
    public async Task GenerateSnapshotAsync_ReturnsHighScoreForRestfulWeek()
    {
        var bingeStore = new InMemoryBingeStore();
        InMemoryAvatarFeedbackRepository repo = new();
        AvatarFeedbackService service = new(bingeStore, repo);

        var snapshot = await service.GenerateSnapshotAsync("alex", ReferenceDate);

        Assert.Equal(100, snapshot.WeeklyScore);
        Assert.Equal(AvatarEnergyState.Energized, snapshot.EnergyState);
        Assert.Equal("avatar_energized_glow", snapshot.AnimationKey);
    }

    [Fact]
    public async Task GenerateSnapshotAsync_ShiftsToTiredWhenWeeklyScoreDrops()
    {
        var entries = Enumerable.Range(0, 9)
            .Select(offset => new BingeEntry
            {
                Username = "alex",
                Date = ReferenceDate.Date.AddDays(-offset).AddHours(3),
                Duration = TimeSpan.FromMinutes(10)
            })
            .ToArray();

        var bingeStore = new InMemoryBingeStore(entries);
        InMemoryAvatarFeedbackRepository repo = new();
        AvatarFeedbackService service = new(bingeStore, repo);

        var snapshot = await service.GenerateSnapshotAsync("alex", ReferenceDate);

        Assert.True(snapshot.WeeklyScore < 55, "Weekly score should drop below steady threshold.");
        Assert.Equal(AvatarEnergyState.Tired, snapshot.EnergyState);
        Assert.Equal("avatar_tired_rest", snapshot.AnimationKey);
    }

    [Fact]
    public async Task GenerateSnapshotAsync_UsesHidePreference()
    {
        var entries = Array.Empty<BingeEntry>();
        var bingeStore = new InMemoryBingeStore(entries);
        InMemoryAvatarFeedbackRepository repo = new();
        var seeded = AvatarFeedbackSettings.CreateDefault("alex").WithHideAvatar(true);
        await repo.UpsertSettingsAsync(seeded);

        AvatarFeedbackService service = new(bingeStore, repo);

        var snapshot = await service.GenerateSnapshotAsync("alex", ReferenceDate);

        Assert.True(snapshot.IsHidden);
        Assert.Equal(AvatarEnergyState.Hidden, snapshot.EnergyState);
        Assert.Equal("avatar_hidden", snapshot.AnimationKey);
    }

    [Fact]
    public async Task GenerateSnapshotAsync_FlagsMonthlyCelebrationWhenDeltaPositive()
    {
        List<BingeEntry> entries = new();

        // Previous month heavy usage to reduce historical score.
        for (var day = 1; day <= 12; day++)
        {
            entries.Add(new BingeEntry
            {
                Username = "alex",
                Date = new DateTime(2025, 1, day, 10, 0, 0, DateTimeKind.Utc),
                Duration = TimeSpan.FromMinutes(90)
            });
        }

        // Current month is calm.
        entries.Add(new BingeEntry
        {
            Username = "alex",
            Date = new DateTime(2025, 2, 10, 12, 0, 0, DateTimeKind.Utc),
            Duration = TimeSpan.FromMinutes(5)
        });

        var bingeStore = new InMemoryBingeStore(entries);
        InMemoryAvatarFeedbackRepository repo = new();
        AvatarFeedbackService service = new(bingeStore, repo);

        var snapshot = await service.GenerateSnapshotAsync("alex", ReferenceDate);

        Assert.True(snapshot.CelebrateMonthlyWin);
        Assert.Equal(AvatarEnergyState.Energized, snapshot.EnergyState);
        Assert.Equal("avatar_energized_fireworks", snapshot.AnimationKey);
    }

    [Fact]
    public async Task UpdateHidePreferenceAsync_PersistsPreference()
    {
        var bingeStore = new InMemoryBingeStore();
        InMemoryAvatarFeedbackRepository repo = new();
        AvatarFeedbackService service = new(bingeStore, repo);

        await service.UpdateHidePreferenceAsync("alex", hideAvatar: true);
        var settings = await repo.GetSettingsAsync("alex");

        Assert.NotNull(settings);
        Assert.True(settings!.HideAvatar);
    }

    private sealed class InMemoryAvatarFeedbackRepository : IAvatarFeedbackRepository
    {
        private readonly Dictionary<string, AvatarFeedbackSettings> _settings = new(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<(string User, string PeriodType, DateTime PeriodStartUtc), AvatarStateRecord> _records = new();

        public Task<AvatarFeedbackSettings?> GetSettingsAsync(string username)
        {
            _settings.TryGetValue(username.Trim(), out var settings);
            return Task.FromResult(settings);
        }

        public Task UpsertSettingsAsync(AvatarFeedbackSettings settings)
        {
            _settings[settings.Username.Trim()] = settings;
            return Task.CompletedTask;
        }

        public Task<AvatarStateRecord?> GetSnapshotAsync(string username, DateTime periodStartUtc, string periodType)
        {
            _records.TryGetValue((username.Trim(), periodType, periodStartUtc), out var record);
            return Task.FromResult(record);
        }

        public Task<AvatarStateRecord?> GetLatestSnapshotAsync(string username, string periodType)
        {
            var record = _records
                .Where(kv => string.Equals(kv.Key.User, username.Trim(), StringComparison.OrdinalIgnoreCase)
                             && kv.Key.PeriodType == periodType)
                .OrderByDescending(kv => kv.Key.PeriodStartUtc)
                .Select(kv => kv.Value)
                .FirstOrDefault();

            return Task.FromResult(record);
        }

        public Task SaveSnapshotAsync(AvatarStateRecord record)
        {
            _records[(record.Username.Trim(), record.PeriodType, record.PeriodStartUtc)] = record;
            return Task.CompletedTask;
        }
    }
}
