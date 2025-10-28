using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bingie.Models;

namespace Bingie.Services;

public class CalendarService
{
    private readonly IDataStore<BingeEntry> _dataStore;

    public CalendarService(IDataStore<BingeEntry> dataStore)
    {
        _dataStore = dataStore ?? throw new ArgumentNullException(nameof(dataStore));
    }

    public async Task<IDictionary<int, int>> GetMonthlyBingeCountsAsync(string username, DateTime month)
    {
        if (string.IsNullOrWhiteSpace(username)) return new Dictionary<int, int>();

        var normalizedUsername = username.Trim();
        var firstDayOfMonth = new DateTime(month.Year, month.Month, 1);
        var firstDayOfNextMonth = firstDayOfMonth.AddMonths(1);

        IEnumerable<BingeEntry> records = await _dataStore.GetItemsAsync();

        return records
            .Where(r => string.Equals(r.Username, normalizedUsername, StringComparison.OrdinalIgnoreCase)
                        && r.Date >= firstDayOfMonth
                        && r.Date < firstDayOfNextMonth)
            .GroupBy(r => r.Date.Day)
            .ToDictionary(g => g.Key, g => g.Count());
    }

    public async Task<IReadOnlyList<BingeEntry>> GetEntriesForDateAsync(string username, DateTime date)
    {
        if (string.IsNullOrWhiteSpace(username)) return Array.Empty<BingeEntry>();

        var normalizedUsername = username.Trim();
        var dayStart = date.Date;
        var dayEnd = dayStart.AddDays(1);

        IEnumerable<BingeEntry> records = await _dataStore.GetItemsAsync();

        return records
            .Where(r => string.Equals(r.Username, normalizedUsername, StringComparison.OrdinalIgnoreCase)
                        && r.Date >= dayStart
                        && r.Date < dayEnd)
            .OrderBy(r => r.Date)
            .ToList();
    }
}
