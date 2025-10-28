using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bingie.Models;
using Bingie.Services;

namespace Bingie.Tests.TestDoubles;

internal sealed class InMemoryUserStore : IDataStore<User>
{
    private readonly List<User> _users;
    private int _nextId;

    public InMemoryUserStore(IEnumerable<User>? seed = null)
    {
        _users = seed?.Select(Clone).ToList() ?? new List<User>();
        _nextId = _users.Any() ? _users.Max(u => u.Id) + 1 : 1;
    }

    public Task<bool> AddItemAsync(User item)
    {
        var user = Clone(item);
        if (user.Id == 0) user.Id = _nextId++;
        _users.RemoveAll(u => u.Id == user.Id);
        _users.Add(user);
        return Task.FromResult(true);
    }

    public Task<bool> UpdateItemAsync(User item)
    {
        var existingIndex = _users.FindIndex(u => u.Id == item.Id);
        if (existingIndex >= 0)
        {
            _users[existingIndex] = Clone(item);
        }
        return Task.FromResult(true);
    }

    public Task<bool> DeleteItemAsync(string id)
    {
        if (int.TryParse(id, out var numericId))
        {
            _users.RemoveAll(u => u.Id == numericId);
        }
        return Task.FromResult(true);
    }

    public Task<User?> GetItemAsync(string id)
    {
        if (!int.TryParse(id, out var numericId)) return Task.FromResult<User?>(null);
        var match = _users.FirstOrDefault(u => u.Id == numericId);
        return Task.FromResult(match is null ? null : Clone(match));
    }

    public Task<IEnumerable<User>> GetItemsAsync()
    {
        return Task.FromResult<IEnumerable<User>>(_users.Select(Clone).ToList());
    }

    private static User Clone(User user)
    {
        return new User
        {
            Id = user.Id,
            Username = user.Username,
            Password = user.Password,
            RememberToken = user.RememberToken
        };
    }
}

internal sealed class InMemoryBingeStore : IDataStore<BingeEntry>
{
    private readonly List<BingeEntry> _entries;
    private int _nextId;

    public InMemoryBingeStore(IEnumerable<BingeEntry>? seed = null)
    {
        _entries = seed?.Select(Clone).ToList() ?? new List<BingeEntry>();
        _nextId = _entries.Any() ? _entries.Max(e => e.Id) + 1 : 1;
    }

    public Task<bool> AddItemAsync(BingeEntry item)
    {
        var entry = Clone(item);
        if (entry.Id == 0) entry.Id = _nextId++;
        _entries.Add(entry);
        return Task.FromResult(true);
    }

    public Task<bool> UpdateItemAsync(BingeEntry item)
    {
        var index = _entries.FindIndex(e => e.Id == item.Id);
        if (index >= 0) _entries[index] = Clone(item);
        return Task.FromResult(true);
    }

    public Task<bool> DeleteItemAsync(string id)
    {
        if (int.TryParse(id, out var numericId))
        {
            _entries.RemoveAll(e => e.Id == numericId);
        }
        return Task.FromResult(true);
    }

    public Task<BingeEntry?> GetItemAsync(string id)
    {
        if (!int.TryParse(id, out var numericId)) return Task.FromResult<BingeEntry?>(null);
        var match = _entries.FirstOrDefault(e => e.Id == numericId);
        return Task.FromResult(match is null ? null : Clone(match));
    }

    public Task<IEnumerable<BingeEntry>> GetItemsAsync()
    {
        return Task.FromResult<IEnumerable<BingeEntry>>(_entries.Select(Clone).ToList());
    }

    private static BingeEntry Clone(BingeEntry entry)
    {
        return new BingeEntry
        {
            Id = entry.Id,
            Username = entry.Username,
            Date = entry.Date,
            Duration = entry.Duration
        };
    }
}
