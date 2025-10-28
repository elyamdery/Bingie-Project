using Microsoft.Data.Sqlite;

namespace Bingie.Services;

public class DatabaseInitializer
{
    private readonly SqliteConnectionFactory _connectionFactory;

    public DatabaseInitializer(SqliteConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public void InitializeDatabase()
    {
        using var connection = _connectionFactory.CreateConnection();
        connection.Open();

        // Create Users table (unique username)
        using var command = connection.CreateCommand();

        command.CommandText = @"
                CREATE TABLE IF NOT EXISTS Users (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Username TEXT NOT NULL UNIQUE,
                    Password TEXT NOT NULL,
                    RememberToken TEXT
                );";
        _ = command.ExecuteNonQuery();

        TryEnsureRememberTokenColumn(connection);

        command.CommandText = @"
                CREATE TABLE IF NOT EXISTS BingeEntries (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Username TEXT NOT NULL,
                    Date TEXT NOT NULL,
                    Duration TEXT NOT NULL
                );";
        _ = command.ExecuteNonQuery();

        EnsureAvatarFeedbackTables(connection);
    }

    private static void TryEnsureRememberTokenColumn(SqliteConnection connection)
    {
        using var ensureCommand = connection.CreateCommand();
        ensureCommand.CommandText = "ALTER TABLE Users ADD COLUMN RememberToken TEXT;";

        try
        {
            _ = ensureCommand.ExecuteNonQuery();
        }
        catch (SqliteException ex) when (ex.SqliteErrorCode == 1)
        {
            // Column already exists; ignore.
        }
    }

    private static void EnsureAvatarFeedbackTables(SqliteConnection connection)
    {
        using var createCommand = connection.CreateCommand();
        createCommand.CommandText = @"
                CREATE TABLE IF NOT EXISTS AvatarFeedbackSettings (
                    Username TEXT PRIMARY KEY,
                    HideAvatar INTEGER NOT NULL DEFAULT 0,
                    WeeklyGlowThreshold INTEGER NOT NULL,
                    WeeklyConcernThreshold INTEGER NOT NULL,
                    MonthlyGlowThreshold INTEGER NOT NULL,
                    MonthlyConcernThreshold INTEGER NOT NULL,
                    LastUpdatedUtc TEXT NOT NULL
                );";
        _ = createCommand.ExecuteNonQuery();

        createCommand.CommandText = @"
                CREATE TABLE IF NOT EXISTS AvatarStateHistory (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Username TEXT NOT NULL,
                    PeriodStartUtc TEXT NOT NULL,
                    PeriodType TEXT NOT NULL,
                    Score REAL NOT NULL,
                    EnergyState TEXT NOT NULL,
                    DeltaFromPrevious REAL NOT NULL,
                    SupportiveCopy TEXT NOT NULL,
                    CreatedUtc TEXT NOT NULL,
                    UNIQUE(Username, PeriodType, PeriodStartUtc)
                );";
        _ = createCommand.ExecuteNonQuery();

        createCommand.CommandText = @"
                CREATE INDEX IF NOT EXISTS IX_AvatarStateHistory_UserPeriod
                ON AvatarStateHistory (Username, PeriodType, PeriodStartUtc);";
        _ = createCommand.ExecuteNonQuery();
    }
}
