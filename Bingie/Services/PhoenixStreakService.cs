using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bingie.Models;

namespace Bingie.Services;

public sealed class PhoenixStreakService
{
    private readonly IDataStore<BingeEntry> _bingeStore;
    private readonly SqliteConnectionFactory _connectionFactory;

    private const int DefaultGraceTokens = 1;

    public PhoenixStreakService(IDataStore<BingeEntry> bingeStore, SqliteConnectionFactory connectionFactory)
    {
        _bingeStore = bingeStore ?? throw new ArgumentNullException(nameof(bingeStore));
        _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
    }

    public async Task<PhoenixStreakState> GetStateAsync(string username, DateTime nowUtc)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(username);
        var settings = await GetSettingsAsync(username.Trim(), nowUtc);
        settings = await EnsureGraceWindowAsync(settings, nowUtc);

        var entries = await LoadEntriesAsync(username);
        var history = BuildHistory(entries, nowUtc, settings.DailyBingeThreshold);
        var (streak, recoveryNeeded) = CalculateStreak(history, settings.DailyBingeThreshold);

        return new PhoenixStreakState
        {
            CurrentStreakDays = streak,
            RecoveryNeeded = recoveryNeeded && settings.GraceTokens == 0,
            GraceAvailable = settings.GraceTokens > 0,
            GraceTokens = settings.GraceTokens,
            GraceResetUtc = settings.LastGraceResetUtc.AddDays(7),
            Today = nowUtc.Date,
            Threshold = settings.DailyBingeThreshold,
            History = history
        };
    }

    public async Task UseGraceTokenAsync(string username, DateTime nowUtc)
    {
        var settings = await GetSettingsAsync(username.Trim(), nowUtc);
        if (settings.GraceTokens <= 0) return;
        settings = settings.WithGrace(settings.GraceTokens - 1, settings.LastGraceResetUtc);
        await SaveSettingsAsync(settings);
    }

    public async Task UpdateThresholdAsync(string username, int threshold, DateTime nowUtc)
    {
        var settings = await GetSettingsAsync(username.Trim(), nowUtc);
        settings = settings.WithThreshold(threshold, nowUtc);
        await SaveSettingsAsync(settings);
    }

    public async Task UpdateHidePreferenceAsync(string username, bool hide, DateTime nowUtc)
    {
        var settings = await GetSettingsAsync(username.Trim(), nowUtc);
        settings = settings.WithHide(hide, nowUtc);
        await SaveSettingsAsync(settings);
    }

    private async Task<PhoenixStreakSettings> GetSettingsAsync(string username, DateTime nowUtc)
    {
        await using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();
        await using var command = connection.CreateCommand();
        command.CommandText = @"
            SELECT Username, Threshold, HideStreak, LastUpdatedUtc, LastGraceResetUtc, GraceTokens
            FROM PhoenixStreakSettings
            WHERE Username = @username;";
        _ = command.Parameters.AddWithValue("@username", username);

        await using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new PhoenixStreakSettings
            {
                Username = reader.GetString(0),
                DailyBingeThreshold = reader.GetInt32(1),
                HideStreak = reader.GetInt32(2) != 0,
                LastUpdatedUtc = DateTime.Parse(reader.GetString(3)),
                LastGraceResetUtc = DateTime.Parse(reader.GetString(4)),
                GraceTokens = reader.GetInt32(5)
            };
        }

        var created = PhoenixStreakSettings.CreateDefault(username, nowUtc);
        await SaveSettingsAsync(created);
        return created;
    }

    private async Task SaveSettingsAsync(PhoenixStreakSettings settings)
    {
        await using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();
        await using var command = connection.CreateCommand();
        command.CommandText = @"
            INSERT INTO PhoenixStreakSettings (Username, Threshold, HideStreak, LastUpdatedUtc, LastGraceResetUtc, GraceTokens)
            VALUES (@username, @threshold, @hide, @updated, @graceReset, @tokens)
            ON CONFLICT(Username)
            DO UPDATE SET
                Threshold = excluded.Threshold,
                HideStreak = excluded.HideStreak,
                LastUpdatedUtc = excluded.LastUpdatedUtc,
                LastGraceResetUtc = excluded.LastGraceResetUtc,
                GraceTokens = excluded.GraceTokens;";

        _ = command.Parameters.AddWithValue("@username", settings.Username);
        _ = command.Parameters.AddWithValue("@threshold", settings.DailyBingeThreshold);
        _ = command.Parameters.AddWithValue("@hide", settings.HideStreak ? 1 : 0);
        _ = command.Parameters.AddWithValue("@updated", settings.LastUpdatedUtc.ToString("O"));
        _ = command.Parameters.AddWithValue("@graceReset", settings.LastGraceResetUtc.ToString("O"));
        _ = command.Parameters.AddWithValue("@tokens", settings.GraceTokens);
        await command.ExecuteNonQueryAsync();
    }

    private async Task<PhoenixStreakSettings> EnsureGraceWindowAsync(PhoenixStreakSettings settings, DateTime nowUtc)
    {
        var currentWindow = StartOfWeek(nowUtc);
        if (settings.LastGraceResetUtc < currentWindow)
        {
            settings = settings.WithGrace(DefaultGraceTokens, currentWindow);
            await SaveSettingsAsync(settings);
        }

        return settings;
    }

    private async Task<List<BingeEntry>> LoadEntriesAsync(string username)
    {
        IEnumerable<BingeEntry> items = await _bingeStore.GetItemsAsync();
        return items
            .Where(entry => string.Equals(entry.Username, username, StringComparison.OrdinalIgnoreCase))
            .OrderBy(entry => entry.Date)
            .ToList();
    }

    private static List<PhoenixStreakDay> BuildHistory(IReadOnlyCollection<BingeEntry> entries, DateTime nowUtc, int threshold)
    {
        List<PhoenixStreakDay> history = new();
        var today = nowUtc.Date;
        var lookup = entries
            .GroupBy(e => e.Date.ToLocalTime().Date)
            .ToDictionary(g => g.Key, g => g.Count());

        for (var offset = 0; offset < 14; offset++)
        {
            var date = today.AddDays(-offset);
            lookup.TryGetValue(date, out var count);
            history.Add(new PhoenixStreakDay
            {
                Date = date,
                BingeCount = count,
                UnderThreshold = count <= threshold
            });
        }

        history.Reverse();
        return history;
    }

    private static (int Streak, bool RecoveryNeeded) CalculateStreak(IReadOnlyList<PhoenixStreakDay> history, int threshold)
    {
        var streak = 0;
        var recoveryNeeded = false;
        for (var i = history.Count - 1; i >= 0; i--)
        {
            if (history[i].UnderThreshold)
            {
                streak++;
                continue;
            }

            recoveryNeeded = true;
            break;
        }

        return (streak, recoveryNeeded);
    }

    private static DateTime StartOfWeek(DateTime nowUtc)
    {
        var offset = ((int)nowUtc.DayOfWeek + 6) % 7;
        return nowUtc.Date.AddDays(-offset);
    }
}
