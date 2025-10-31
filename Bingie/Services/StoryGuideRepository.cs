using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Bingie.Models;
using Microsoft.Data.Sqlite;

namespace Bingie.Services;

public sealed class StoryGuideRepository : IStoryGuideRepository
{
    private readonly SqliteConnectionFactory _connectionFactory;

    public StoryGuideRepository(SqliteConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
    }

    public async Task UpsertTriggerSelectionAsync(StoryTriggerSelection selection)
    {
        ArgumentNullException.ThrowIfNull(selection);

        await using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        var command = connection.CreateCommand();
        command.CommandText = @"
            INSERT INTO StoryTriggerSelections (
                Username,
                EntryDateUtc,
                TriggerCode,
                CustomTrigger,
                CreatedUtc
            )
            VALUES (
                @username,
                @entryDateUtc,
                @triggerCode,
                @customTrigger,
                @createdUtc
            )
            ON CONFLICT(Username, EntryDateUtc)
            DO UPDATE SET
                TriggerCode = excluded.TriggerCode,
                CustomTrigger = excluded.CustomTrigger,
                CreatedUtc = excluded.CreatedUtc;";

        AddSelectionParameters(command, selection);
        _ = await command.ExecuteNonQueryAsync();
    }

    public async Task<StoryTriggerSelection?> GetTriggerSelectionAsync(string username, DateTime entryDateUtc)
    {
        if (string.IsNullOrWhiteSpace(username)) return null;

        await using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        var command = connection.CreateCommand();
        command.CommandText = @"
            SELECT Id,
                   Username,
                   EntryDateUtc,
                   TriggerCode,
                   CustomTrigger,
                   CreatedUtc
            FROM StoryTriggerSelections
            WHERE Username = @username
              AND EntryDateUtc = @entryDateUtc
            LIMIT 1;";

        _ = command.Parameters.AddWithValue("@username", username.Trim());
        _ = command.Parameters.AddWithValue("@entryDateUtc", entryDateUtc.ToString("O"));

        await using var reader = await command.ExecuteReaderAsync();
        if (!await reader.ReadAsync()) return null;

        return MapSelection(reader);
    }

    public async Task<IReadOnlyList<StoryTriggerSelection>> GetTriggerSelectionsAsync(string username, DateTime fromUtc, DateTime toUtc)
    {
        await using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        var command = connection.CreateCommand();
        command.CommandText = @"
            SELECT Id,
                   Username,
                   EntryDateUtc,
                   TriggerCode,
                   CustomTrigger,
                   CreatedUtc
            FROM StoryTriggerSelections
            WHERE Username = @username
              AND EntryDateUtc >= @fromUtc
              AND EntryDateUtc < @toUtc
            ORDER BY EntryDateUtc ASC;";

        _ = command.Parameters.AddWithValue("@username", username.Trim());
        _ = command.Parameters.AddWithValue("@fromUtc", fromUtc.ToString("O"));
        _ = command.Parameters.AddWithValue("@toUtc", toUtc.ToString("O"));

        List<StoryTriggerSelection> results = new();
        await using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            results.Add(MapSelection(reader));
        }

        return results;
    }

    public async Task<StoryExperimentProgress?> GetExperimentProgressAsync(string username, string experimentCode)
    {
        if (string.IsNullOrWhiteSpace(username)) return null;
        if (string.IsNullOrWhiteSpace(experimentCode)) return null;

        await using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        var command = connection.CreateCommand();
        command.CommandText = @"
            SELECT Id,
                   Username,
                   ExperimentCode,
                   Status,
                   LastSuggestedUtc,
                   PlannedUtc,
                   CompletedUtc,
                   XpGranted
            FROM StoryExperimentProgress
            WHERE Username = @username AND ExperimentCode = @experimentCode
            LIMIT 1;";

        _ = command.Parameters.AddWithValue("@username", username.Trim());
        _ = command.Parameters.AddWithValue("@experimentCode", experimentCode);

        await using var reader = await command.ExecuteReaderAsync();
        return await reader.ReadAsync() ? MapExperiment(reader) : null;
    }

    public async Task<IReadOnlyList<StoryExperimentProgress>> GetExperimentProgressAsync(string username)
    {
        await using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        var command = connection.CreateCommand();
        command.CommandText = @"
            SELECT Id,
                   Username,
                   ExperimentCode,
                   Status,
                   LastSuggestedUtc,
                   PlannedUtc,
                   CompletedUtc,
                   XpGranted
            FROM StoryExperimentProgress
            WHERE Username = @username;";

        _ = command.Parameters.AddWithValue("@username", username.Trim());

        List<StoryExperimentProgress> results = new();
        await using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            results.Add(MapExperiment(reader));
        }

        return results;
    }

    public async Task UpsertExperimentProgressAsync(StoryExperimentProgress progress)
    {
        ArgumentNullException.ThrowIfNull(progress);

        await using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        var command = connection.CreateCommand();
        command.CommandText = @"
            INSERT INTO StoryExperimentProgress (
                Username,
                ExperimentCode,
                Status,
                LastSuggestedUtc,
                PlannedUtc,
                CompletedUtc,
                XpGranted
            )
            VALUES (
                @username,
                @code,
                @status,
                @lastSuggested,
                @planned,
                @completed,
                @xpGranted
            )
            ON CONFLICT(Username, ExperimentCode)
            DO UPDATE SET
                Status = excluded.Status,
                LastSuggestedUtc = excluded.LastSuggestedUtc,
                PlannedUtc = excluded.PlannedUtc,
                CompletedUtc = excluded.CompletedUtc,
                XpGranted = excluded.XpGranted;";

        AddExperimentParameters(command, progress);
        _ = await command.ExecuteNonQueryAsync();
    }

    private static void AddSelectionParameters(SqliteCommand command, StoryTriggerSelection selection)
    {
        _ = command.Parameters.AddWithValue("@username", selection.Username.Trim());
        _ = command.Parameters.AddWithValue("@entryDateUtc", selection.EntryDateUtc.ToString("O"));
        _ = command.Parameters.AddWithValue("@triggerCode", selection.TriggerCode);
        _ = command.Parameters.AddWithValue("@customTrigger", (object?)selection.CustomTrigger ?? DBNull.Value);
        _ = command.Parameters.AddWithValue("@createdUtc", selection.CreatedUtc.ToString("O"));
    }

    private static void AddExperimentParameters(SqliteCommand command, StoryExperimentProgress progress)
    {
        _ = command.Parameters.AddWithValue("@username", progress.Username.Trim());
        _ = command.Parameters.AddWithValue("@code", progress.ExperimentCode);
        _ = command.Parameters.AddWithValue("@status", (int)progress.Status);
        _ = command.Parameters.AddWithValue("@lastSuggested", FormatNullable(progress.LastSuggestedUtc));
        _ = command.Parameters.AddWithValue("@planned", FormatNullable(progress.PlannedUtc));
        _ = command.Parameters.AddWithValue("@completed", FormatNullable(progress.CompletedUtc));
        _ = command.Parameters.AddWithValue("@xpGranted", progress.XpGranted ? 1 : 0);
    }

    private static StoryTriggerSelection MapSelection(SqliteDataReader reader)
    {
        return new StoryTriggerSelection
        {
            Id = reader.GetInt64(0),
            Username = reader.GetString(1),
            EntryDateUtc = ParseDate(reader.GetString(2)),
            TriggerCode = reader.GetString(3),
            CustomTrigger = reader.IsDBNull(4) ? null : reader.GetString(4),
            CreatedUtc = ParseDate(reader.GetString(5))
        };
    }

    private static StoryExperimentProgress MapExperiment(SqliteDataReader reader)
    {
        return new StoryExperimentProgress
        {
            Id = reader.GetInt64(0),
            Username = reader.GetString(1),
            ExperimentCode = reader.GetString(2),
            Status = (StoryExperimentStatus)reader.GetInt32(3),
            LastSuggestedUtc = ReadNullable(reader, 4),
            PlannedUtc = ReadNullable(reader, 5),
            CompletedUtc = ReadNullable(reader, 6),
            XpGranted = reader.GetInt32(7) != 0
        };
    }

    private static string? FormatNullable(DateTime? value)
    {
        return value?.ToString("O", CultureInfo.InvariantCulture);
    }

    private static DateTime ParseDate(string value)
    {
        return DateTime.Parse(value, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);
    }

    private static DateTime? ReadNullable(SqliteDataReader reader, int ordinal)
    {
        return reader.IsDBNull(ordinal)
            ? null
            : ParseDate(reader.GetString(ordinal));
    }
}
