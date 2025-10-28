using Bingie.Models;
using Bingie.Services;
using Bingie.Tests.TestDoubles;

namespace Bingie.Tests;

public class CalendarServiceTests
{
    [Fact]
    public async Task GetEntriesForDateAsync_ReturnsOrderedEntriesForMatchingUser()
    {
        // Arrange
        var date = new DateTime(2024, 10, 15, 12, 0, 0, DateTimeKind.Utc);
        var store = new InMemoryBingeStore(new[]
        {
            new BingeEntry { Id = 1, Username = "Alex", Date = date.AddHours(-2), Duration = TimeSpan.Zero },
            new BingeEntry { Id = 2, Username = "alex", Date = date.AddHours(-1), Duration = TimeSpan.Zero },
            new BingeEntry { Id = 3, Username = "sam", Date = date, Duration = TimeSpan.Zero },
            new BingeEntry { Id = 4, Username = "alex", Date = date.AddDays(1), Duration = TimeSpan.Zero }
        });

        var service = new CalendarService(store);

        // Act
        var entries = await service.GetEntriesForDateAsync(" alex ", date.Date);

        // Assert
        Assert.Equal(2, entries.Count);
        Assert.True(entries[0].Date <= entries[1].Date, "Entries should be ordered ascending by time.");
        Assert.All(entries, e => Assert.Equal("alex", e.Username, ignoreCase: true));
    }

    [Fact]
    public async Task GetMonthlyBingeCountsAsync_ComputesCountsPerDay()
    {
        // Arrange
        var month = new DateTime(2024, 7, 1, 0, 0, 0, DateTimeKind.Utc);
        var store = new InMemoryBingeStore(new[]
        {
            new BingeEntry { Id = 1, Username = "maria", Date = new DateTime(2024, 7, 2, 8, 0, 0, DateTimeKind.Utc), Duration = TimeSpan.Zero },
            new BingeEntry { Id = 2, Username = "maria", Date = new DateTime(2024, 7, 2, 20, 0, 0, DateTimeKind.Utc), Duration = TimeSpan.Zero },
            new BingeEntry { Id = 3, Username = "maria", Date = new DateTime(2024, 7, 10, 12, 0, 0, DateTimeKind.Utc), Duration = TimeSpan.Zero },
            new BingeEntry { Id = 4, Username = "maria", Date = new DateTime(2024, 8, 1, 12, 0, 0, DateTimeKind.Utc), Duration = TimeSpan.Zero },
            new BingeEntry { Id = 5, Username = "lucas", Date = new DateTime(2024, 7, 2, 8, 0, 0, DateTimeKind.Utc), Duration = TimeSpan.Zero }
        });

        var service = new CalendarService(store);

        // Act
        var counts = await service.GetMonthlyBingeCountsAsync("Maria", month);

        // Assert
        Assert.Equal(2, counts.Count);
        Assert.Equal(2, counts[2]);
        Assert.Equal(1, counts[10]);
        Assert.DoesNotContain(1, counts.Keys);
    }
}

