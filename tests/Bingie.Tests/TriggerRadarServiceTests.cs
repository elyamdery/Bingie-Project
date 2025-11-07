using Bingie.Models;
using Bingie.Services;

namespace Bingie.Tests;

public class TriggerRadarServiceTests
{
    [Fact]
    public async Task GetAsync_ReturnsNull_WhenNoEntries()
    {
        var store = new InMemoryBingeStore(Array.Empty<BingeEntry>());
        TriggerRadarService service = new(store);

        var result = await service.GetAsync("alex", DateTime.UtcNow);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetAsync_ComputesBucketsAndSuggestions()
    {
        var now = new DateTime(2025, 3, 30, 18, 0, 0, DateTimeKind.Utc);
        List<BingeEntry> entries = new();
        for (var i = 0; i < 6; i++)
        {
            entries.Add(new BingeEntry
            {
                Id = i + 1,
                Username = "alex",
                Date = now.AddDays(-(i % 3)).AddHours(-i),
                Duration = TimeSpan.Zero
            });
        }

        var store = new InMemoryBingeStore(entries);
        TriggerRadarService service = new(store);

        var result = await service.GetAsync("alex", now);

        Assert.NotNull(result);
        Assert.NotEmpty(result!.TimeOfDayBuckets);
        Assert.True(result.Suggestions.Count > 0);
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
