using Bingie.Models;
using Bingie.Services;

namespace Bingie.Tests;

public class PhoenixStreakServiceTests
{
    [Fact]
    public async Task GetStateAsync_NoEntries_ReturnsZeroStreak()
    {
        InMemoryBingeStore store = new(Array.Empty<BingeEntry>());
        PhoenixStreakService service = new(store, new SqliteConnectionFactory());

        var state = await service.GetStateAsync("alex", DateTime.UtcNow);

        Assert.Equal(0, state.CurrentStreakDays);
        Assert.False(state.RecoveryNeeded);
    }

    [Fact]
    public async Task GetStateAsync_UnderThresholdBuildsStreak()
    {
        var now = DateTime.UtcNow;
        var entries = Enumerable.Range(0, 3)
            .Select(i => new BingeEntry { Id = i + 1, Username = "alex", Date = now.AddDays(-i), Duration = TimeSpan.Zero })
            .ToList();
        InMemoryBingeStore store = new(entries);
        PhoenixStreakService service = new(store, new SqliteConnectionFactory());

        var state = await service.GetStateAsync("alex", now);

        Assert.True(state.CurrentStreakDays >= 1);
        Assert.False(state.RecoveryNeeded);
    }

    [Fact]
    public async Task UseGraceToken_ReducesTokenCount()
    {
        var now = DateTime.UtcNow;
        var entries = new[]
        {
            new BingeEntry { Id = 1, Username = "alex", Date = now, Duration = TimeSpan.FromMinutes(10) },
            new BingeEntry { Id = 2, Username = "alex", Date = now.AddDays(-1), Duration = TimeSpan.FromMinutes(10) }
        };
        InMemoryBingeStore store = new(entries);
        PhoenixStreakService service = new(store, new SqliteConnectionFactory());

        var before = await service.GetStateAsync("alex", now);
        Assert.True(before.GraceAvailable);

        await service.UseGraceTokenAsync("alex", now);
        var after = await service.GetStateAsync("alex", now);

        Assert.False(after.GraceAvailable);
    }
    private sealed class InMemoryBingeStore : IDataStore<BingeEntry>
    {
        private readonly List<BingeEntry> _entries;

        public InMemoryBingeStore(IEnumerable<BingeEntry> entries)
        {
            _entries = entries.ToList();
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
