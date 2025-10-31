using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Bingie.Models;
using Microsoft.Data.Sqlite;

namespace Bingie.Services;

public sealed class PointsSystemRepository : IPointsSystemRepository
{
    private readonly SqliteConnectionFactory _connectionFactory;

    public PointsSystemRepository(SqliteConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
    }

    public async Task<IReadOnlyList<PointActionDefinition>> GetActionDefinitionsAsync()
    {
        List<PointActionDefinition> actions = new();
        await using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        var command = connection.CreateCommand();
        command.CommandText = @"SELECT ActionCode, Title, Description, XpValue, DailyLimit, QuestEligible FROM PointActions";

        await using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            actions.Add(new PointActionDefinition
            {
                ActionCode = reader.GetString(0),
                Title = reader.GetString(1),
                Description = reader.GetString(2),
                XpValue = reader.GetInt32(3),
                DailyLimit = reader.GetInt32(4),
                QuestEligible = reader.GetInt32(5) != 0
            });
        }

        return actions;
    }

    public async Task<IReadOnlyList<CosmeticReward>> GetCosmeticRewardsAsync()
    {
        List<CosmeticReward> rewards = new();
        await using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        var command = connection.CreateCommand();
        command.CommandText = @"SELECT CosmeticCode, Name, Description, RequiredXp FROM PointCosmetics ORDER BY RequiredXp";

        await using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            rewards.Add(new CosmeticReward
            {
                CosmeticCode = reader.GetString(0),
                Name = reader.GetString(1),
                Description = reader.GetString(2),
                RequiredXp = reader.GetInt32(3)
            });
        }

        return rewards;
    }

    public async Task<PointsSettings?> GetSettingsAsync(string username)
    {
        await using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        var command = connection.CreateCommand();
        command.CommandText = @"SELECT Username, RewardsPaused, EquippedCosmeticCode FROM PointSettings WHERE Username = @username";
        _ = command.Parameters.AddWithValue("@username", username.Trim());

        await using var reader = await command.ExecuteReaderAsync();
        if (!await reader.ReadAsync()) return null;

        return new PointsSettings
        {
            Username = reader.GetString(0),
            RewardsPaused = reader.GetInt32(1) != 0,
            EquippedCosmeticCode = reader.IsDBNull(2) ? null : reader.GetString(2)
        };
    }

    public async Task UpsertSettingsAsync(PointsSettings settings)
    {
        ArgumentNullException.ThrowIfNull(settings);
        await using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        var command = connection.CreateCommand();
        command.CommandText = @"
            INSERT INTO PointSettings (Username, RewardsPaused, EquippedCosmeticCode)
            VALUES (@username, @paused, @equipped)
            ON CONFLICT(Username)
            DO UPDATE SET
                RewardsPaused = excluded.RewardsPaused,
                EquippedCosmeticCode = excluded.EquippedCosmeticCode;";

        _ = command.Parameters.AddWithValue("@username", settings.Username.Trim());
        _ = command.Parameters.AddWithValue("@paused", settings.RewardsPaused ? 1 : 0);
        _ = command.Parameters.AddWithValue("@equipped", (object?)settings.EquippedCosmeticCode ?? DBNull.Value);

        await command.ExecuteNonQueryAsync();
    }

    public async Task<IReadOnlyList<DailyQuestRecord>> GetDailyQuestsAsync(string username, DateTime dayStartUtc, DateTime dayEndUtc)
    {
        List<DailyQuestRecord> quests = new();
        await using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        var command = connection.CreateCommand();
        command.CommandText = @"
            SELECT Id, Username, ActionCode, QuestDateUtc, CreatedUtc, CompletedUtc, XpAwarded
            FROM PointDailyQuests
            WHERE Username = @username AND QuestDateUtc >= @from AND QuestDateUtc < @to
            ORDER BY Id;";

        _ = command.Parameters.AddWithValue("@username", username.Trim());
        _ = command.Parameters.AddWithValue("@from", dayStartUtc.ToString("O", CultureInfo.InvariantCulture));
        _ = command.Parameters.AddWithValue("@to", dayEndUtc.ToString("O", CultureInfo.InvariantCulture));

        await using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            quests.Add(MapQuest(reader));
        }

        return quests;
    }

    public async Task<long> AddDailyQuestAsync(DailyQuestRecord quest)
    {
        ArgumentNullException.ThrowIfNull(quest);
        await using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        var command = connection.CreateCommand();
        command.CommandText = @"
            INSERT INTO PointDailyQuests (Username, ActionCode, QuestDateUtc, CreatedUtc, CompletedUtc, XpAwarded)
            VALUES (@username, @actionCode, @questDateUtc, @createdUtc, @completedUtc, @xpAwarded);
            SELECT last_insert_rowid();";

        AddQuestParameters(command, quest);

        var result = await command.ExecuteScalarAsync();
        return Convert.ToInt64(result, CultureInfo.InvariantCulture);
    }

    public async Task UpdateDailyQuestAsync(DailyQuestRecord quest)
    {
        ArgumentNullException.ThrowIfNull(quest);
        await using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        var command = connection.CreateCommand();
        command.CommandText = @"
            UPDATE PointDailyQuests
            SET CompletedUtc = @completedUtc,
                XpAwarded = @xpAwarded
            WHERE Id = @id;";

        _ = command.Parameters.AddWithValue("@completedUtc", quest.CompletedUtc?.ToString("O", CultureInfo.InvariantCulture) ?? (object)DBNull.Value);
        _ = command.Parameters.AddWithValue("@xpAwarded", quest.XpAwarded);
        _ = command.Parameters.AddWithValue("@id", quest.Id);

        await command.ExecuteNonQueryAsync();
    }

    public async Task<DailyQuestRecord?> GetDailyQuestByIdAsync(long questId)
    {
        await using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        var command = connection.CreateCommand();
        command.CommandText = @"
            SELECT Id, Username, ActionCode, QuestDateUtc, CreatedUtc, CompletedUtc, XpAwarded
            FROM PointDailyQuests
            WHERE Id = @id;";

        _ = command.Parameters.AddWithValue("@id", questId);

        await using var reader = await command.ExecuteReaderAsync();
        return await reader.ReadAsync() ? MapQuest(reader) : null;
    }

    public async Task AddXpTransactionAsync(XpTransaction transaction)
    {
        ArgumentNullException.ThrowIfNull(transaction);
        await using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        var command = connection.CreateCommand();
        command.CommandText = @"
            INSERT INTO PointXpTransactions (Username, ActionCode, Amount, CreatedUtc)
            VALUES (@username, @actionCode, @amount, @createdUtc);";

        _ = command.Parameters.AddWithValue("@username", transaction.Username.Trim());
        _ = command.Parameters.AddWithValue("@actionCode", transaction.ActionCode);
        _ = command.Parameters.AddWithValue("@amount", transaction.Amount);
        _ = command.Parameters.AddWithValue("@createdUtc", transaction.CreatedUtc.ToString("O", CultureInfo.InvariantCulture));

        await command.ExecuteNonQueryAsync();
    }

    public async Task<int> GetTotalXpAsync(string username)
    {
        await using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        var command = connection.CreateCommand();
        command.CommandText = @"SELECT COALESCE(SUM(Amount), 0) FROM PointXpTransactions WHERE Username = @username";
        _ = command.Parameters.AddWithValue("@username", username.Trim());

        var result = await command.ExecuteScalarAsync();
        return Convert.ToInt32(result ?? 0, CultureInfo.InvariantCulture);
    }

    public async Task<IReadOnlyList<XpTransaction>> GetXpTransactionsAsync(string username, DateTime fromUtc, DateTime toUtc)
    {
        List<XpTransaction> transactions = new();
        await using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        var command = connection.CreateCommand();
        command.CommandText = @"
            SELECT Id, Username, ActionCode, Amount, CreatedUtc
            FROM PointXpTransactions
            WHERE Username = @username AND CreatedUtc >= @from AND CreatedUtc < @to
            ORDER BY CreatedUtc;";

        _ = command.Parameters.AddWithValue("@username", username.Trim());
        _ = command.Parameters.AddWithValue("@from", fromUtc.ToString("O", CultureInfo.InvariantCulture));
        _ = command.Parameters.AddWithValue("@to", toUtc.ToString("O", CultureInfo.InvariantCulture));

        await using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            transactions.Add(MapTransaction(reader));
        }

        return transactions;
    }

    public async Task<int> GetActionUsageCountAsync(string username, string actionCode, DateTime fromUtc, DateTime toUtc)
    {
        await using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        var command = connection.CreateCommand();
        command.CommandText = @"
            SELECT COUNT(*) FROM PointXpTransactions
            WHERE Username = @username AND ActionCode = @actionCode AND CreatedUtc >= @from AND CreatedUtc < @to;";

        _ = command.Parameters.AddWithValue("@username", username.Trim());
        _ = command.Parameters.AddWithValue("@actionCode", actionCode);
        _ = command.Parameters.AddWithValue("@from", fromUtc.ToString("O", CultureInfo.InvariantCulture));
        _ = command.Parameters.AddWithValue("@to", toUtc.ToString("O", CultureInfo.InvariantCulture));

        var result = await command.ExecuteScalarAsync();
        return Convert.ToInt32(result ?? 0, CultureInfo.InvariantCulture);
    }

    public async Task<IReadOnlyList<CosmeticUnlockState>> GetCosmeticUnlockStatesAsync(string username)
    {
        List<CosmeticUnlockState> states = new();
        await using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        var command = connection.CreateCommand();
        command.CommandText = @"
            SELECT Id, Username, CosmeticCode, UnlockedUtc, Equipped
            FROM PointCosmeticUnlocks
            WHERE Username = @username;";
        _ = command.Parameters.AddWithValue("@username", username.Trim());

        await using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            states.Add(MapCosmeticState(reader));
        }

        return states;
    }

    public async Task UpsertCosmeticUnlockStateAsync(CosmeticUnlockState state)
    {
        ArgumentNullException.ThrowIfNull(state);
        await using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        var command = connection.CreateCommand();
        command.CommandText = @"
            INSERT INTO PointCosmeticUnlocks (Username, CosmeticCode, UnlockedUtc, Equipped)
            VALUES (@username, @cosmeticCode, @unlockedUtc, @equipped)
            ON CONFLICT(Username, CosmeticCode)
            DO UPDATE SET
                UnlockedUtc = excluded.UnlockedUtc,
                Equipped = excluded.Equipped;";

        _ = command.Parameters.AddWithValue("@username", state.Username.Trim());
        _ = command.Parameters.AddWithValue("@cosmeticCode", state.CosmeticCode);
        _ = command.Parameters.AddWithValue("@unlockedUtc", state.UnlockedUtc.ToString("O", CultureInfo.InvariantCulture));
        _ = command.Parameters.AddWithValue("@equipped", state.Equipped ? 1 : 0);

        await command.ExecuteNonQueryAsync();
    }

    public async Task ClearEquippedCosmeticsAsync(string username)
    {
        await using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        var command = connection.CreateCommand();
        command.CommandText = @"UPDATE PointCosmeticUnlocks SET Equipped = 0 WHERE Username = @username";
        _ = command.Parameters.AddWithValue("@username", username.Trim());

        await command.ExecuteNonQueryAsync();
    }

    private static DailyQuestRecord MapQuest(SqliteDataReader reader)
    {
        return new DailyQuestRecord
        {
            Id = reader.GetInt64(0),
            Username = reader.GetString(1),
            ActionCode = reader.GetString(2),
            QuestDateUtc = ParseUtc(reader.GetString(3)),
            CreatedUtc = ParseUtc(reader.GetString(4)),
            CompletedUtc = reader.IsDBNull(5) ? null : ParseUtc(reader.GetString(5)),
            XpAwarded = reader.GetInt32(6)
        };
    }

    private static XpTransaction MapTransaction(SqliteDataReader reader)
    {
        return new XpTransaction
        {
            Id = reader.GetInt64(0),
            Username = reader.GetString(1),
            ActionCode = reader.GetString(2),
            Amount = reader.GetInt32(3),
            CreatedUtc = ParseUtc(reader.GetString(4))
        };
    }

    private static CosmeticUnlockState MapCosmeticState(SqliteDataReader reader)
    {
        return new CosmeticUnlockState
        {
            Id = reader.GetInt64(0),
            Username = reader.GetString(1),
            CosmeticCode = reader.GetString(2),
            UnlockedUtc = ParseUtc(reader.GetString(3)),
            Equipped = reader.GetInt32(4) != 0
        };
    }

    private static void AddQuestParameters(SqliteCommand command, DailyQuestRecord quest)
    {
        _ = command.Parameters.AddWithValue("@username", quest.Username.Trim());
        _ = command.Parameters.AddWithValue("@actionCode", quest.ActionCode);
        _ = command.Parameters.AddWithValue("@questDateUtc", quest.QuestDateUtc.ToString("O", CultureInfo.InvariantCulture));
        _ = command.Parameters.AddWithValue("@createdUtc", quest.CreatedUtc.ToString("O", CultureInfo.InvariantCulture));
        _ = command.Parameters.AddWithValue("@completedUtc", quest.CompletedUtc?.ToString("O", CultureInfo.InvariantCulture) ?? (object)DBNull.Value);
        _ = command.Parameters.AddWithValue("@xpAwarded", quest.XpAwarded);
    }

    private static DateTime ParseUtc(string value)
    {
        return DateTime.Parse(value, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);
    }
}
