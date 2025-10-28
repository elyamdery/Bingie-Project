using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Bingie.Models;
using Microsoft.Data.Sqlite;

namespace Bingie.Services;

/// <summary>
/// Low-level persistence for avatar feedback feature.
/// </summary>
public sealed class AvatarFeedbackRepository : IAvatarFeedbackRepository
{
    private readonly SqliteConnectionFactory _connectionFactory;

    public AvatarFeedbackRepository(SqliteConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
    }

    public async Task<AvatarFeedbackSettings?> GetSettingsAsync(string username)
    {
        if (string.IsNullOrWhiteSpace(username)) return null;

        await using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        var command = connection.CreateCommand();
        command.CommandText = @"
            SELECT Username,
                   HideAvatar,
                   WeeklyGlowThreshold,
                   WeeklyConcernThreshold,
                   MonthlyGlowThreshold,
                   MonthlyConcernThreshold,
                   LastUpdatedUtc
            FROM AvatarFeedbackSettings
            WHERE Username = @username;";
        _ = command.Parameters.AddWithValue("@username", username.Trim());

        await using var reader = await command.ExecuteReaderAsync();
        if (!await reader.ReadAsync()) return null;

        var hideValue = reader.GetFieldType(1) == typeof(bool)
            ? reader.GetBoolean(1)
            : reader.GetInt32(1) != 0;

        return new AvatarFeedbackSettings
        {
            Username = reader.GetString(0),
            HideAvatar = hideValue,
            WeeklyGlowThreshold = reader.GetInt32(2),
            WeeklyConcernThreshold = reader.GetInt32(3),
            MonthlyGlowThreshold = reader.GetInt32(4),
            MonthlyConcernThreshold = reader.GetInt32(5),
            LastUpdatedUtc = DateTime.Parse(reader.GetString(6), CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind)
        };
    }

    public async Task UpsertSettingsAsync(AvatarFeedbackSettings settings)
    {
        ArgumentNullException.ThrowIfNull(settings);

        await using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        var command = connection.CreateCommand();
        command.CommandText = @"
            INSERT INTO AvatarFeedbackSettings (
                Username,
                HideAvatar,
                WeeklyGlowThreshold,
                WeeklyConcernThreshold,
                MonthlyGlowThreshold,
                MonthlyConcernThreshold,
                LastUpdatedUtc
            )
            VALUES (
                @username,
                @hideAvatar,
                @weeklyGlow,
                @weeklyConcern,
                @monthlyGlow,
                @monthlyConcern,
                @lastUpdated
            )
            ON CONFLICT(Username)
            DO UPDATE SET
                HideAvatar = excluded.HideAvatar,
                WeeklyGlowThreshold = excluded.WeeklyGlowThreshold,
                WeeklyConcernThreshold = excluded.WeeklyConcernThreshold,
                MonthlyGlowThreshold = excluded.MonthlyGlowThreshold,
                MonthlyConcernThreshold = excluded.MonthlyConcernThreshold,
                LastUpdatedUtc = excluded.LastUpdatedUtc;";

        AddSettingsParameters(command, settings);
        _ = await command.ExecuteNonQueryAsync();
    }

    public async Task<AvatarStateRecord?> GetSnapshotAsync(string username, DateTime periodStartUtc, string periodType)
    {
        if (string.IsNullOrWhiteSpace(username)) return null;
        ArgumentException.ThrowIfNullOrEmpty(periodType);

        await using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        var command = connection.CreateCommand();
        command.CommandText = @"
            SELECT Id,
                   Username,
                   PeriodStartUtc,
                   PeriodType,
                   Score,
                   EnergyState,
                   DeltaFromPrevious,
                   SupportiveCopy,
                   CreatedUtc
            FROM AvatarStateHistory
            WHERE Username = @username
              AND PeriodType = @periodType
              AND PeriodStartUtc = @periodStart
            LIMIT 1;";

        _ = command.Parameters.AddWithValue("@username", username.Trim());
        _ = command.Parameters.AddWithValue("@periodType", periodType);
        _ = command.Parameters.AddWithValue("@periodStart", periodStartUtc.ToString("O"));

        await using var reader = await command.ExecuteReaderAsync();
        if (!await reader.ReadAsync()) return null;

        return MapRecord(reader);
    }

    public async Task<AvatarStateRecord?> GetLatestSnapshotAsync(string username, string periodType)
    {
        if (string.IsNullOrWhiteSpace(username)) return null;
        ArgumentException.ThrowIfNullOrEmpty(periodType);

        await using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        var command = connection.CreateCommand();
        command.CommandText = @"
            SELECT Id,
                   Username,
                   PeriodStartUtc,
                   PeriodType,
                   Score,
                   EnergyState,
                   DeltaFromPrevious,
                   SupportiveCopy,
                   CreatedUtc
            FROM AvatarStateHistory
            WHERE Username = @username
              AND PeriodType = @periodType
            ORDER BY PeriodStartUtc DESC
            LIMIT 1;";

        _ = command.Parameters.AddWithValue("@username", username.Trim());
        _ = command.Parameters.AddWithValue("@periodType", periodType);

        await using var reader = await command.ExecuteReaderAsync();
        return await reader.ReadAsync() ? MapRecord(reader) : null;
    }

    public async Task SaveSnapshotAsync(AvatarStateRecord record)
    {
        ArgumentNullException.ThrowIfNull(record);

        await using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        var command = connection.CreateCommand();
        command.CommandText = @"
            INSERT INTO AvatarStateHistory (
                Username,
                PeriodStartUtc,
                PeriodType,
                Score,
                EnergyState,
                DeltaFromPrevious,
                SupportiveCopy,
                CreatedUtc
            )
            VALUES (
                @username,
                @periodStart,
                @periodType,
                @score,
                @energyState,
                @delta,
                @copy,
                @createdUtc
            );";

        _ = command.Parameters.AddWithValue("@username", record.Username.Trim());
        _ = command.Parameters.AddWithValue("@periodStart", record.PeriodStartUtc.ToString("O"));
        _ = command.Parameters.AddWithValue("@periodType", record.PeriodType);
        _ = command.Parameters.AddWithValue("@score", record.Score);
        _ = command.Parameters.AddWithValue("@energyState", record.EnergyState.ToString());
        _ = command.Parameters.AddWithValue("@delta", record.DeltaFromPrevious);
        _ = command.Parameters.AddWithValue("@copy", record.SupportiveCopy);
        _ = command.Parameters.AddWithValue("@createdUtc", record.CreatedUtc.ToString("O"));

        _ = await command.ExecuteNonQueryAsync();
    }

    private static void AddSettingsParameters(SqliteCommand command, AvatarFeedbackSettings settings)
    {
        _ = command.Parameters.AddWithValue("@username", settings.Username.Trim());
        _ = command.Parameters.AddWithValue("@hideAvatar", settings.HideAvatar ? 1 : 0);
        _ = command.Parameters.AddWithValue("@weeklyGlow", settings.WeeklyGlowThreshold);
        _ = command.Parameters.AddWithValue("@weeklyConcern", settings.WeeklyConcernThreshold);
        _ = command.Parameters.AddWithValue("@monthlyGlow", settings.MonthlyGlowThreshold);
        _ = command.Parameters.AddWithValue("@monthlyConcern", settings.MonthlyConcernThreshold);
        _ = command.Parameters.AddWithValue("@lastUpdated", settings.LastUpdatedUtc.ToString("O"));
    }

    private static AvatarStateRecord MapRecord(SqliteDataReader reader)
    {
        return new AvatarStateRecord
        {
            Id = reader.GetInt64(0),
            Username = reader.GetString(1),
            PeriodStartUtc = DateTime.Parse(reader.GetString(2), CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind),
            PeriodType = reader.GetString(3),
            Score = reader.GetDouble(4),
            EnergyState = Enum.TryParse<AvatarEnergyState>(reader.GetString(5), out var state) ? state : AvatarEnergyState.Steady,
            DeltaFromPrevious = reader.GetDouble(6),
            SupportiveCopy = reader.GetString(7),
            CreatedUtc = DateTime.Parse(reader.GetString(8), CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind)
        };
    }
}
