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
}
