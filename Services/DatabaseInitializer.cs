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

        // Create Users table
        var createUserTableCmd = @"
                CREATE TABLE IF NOT EXISTS Users (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Username TEXT NOT NULL,
                    Password TEXT NOT NULL
                );";

        using var command = connection.CreateCommand();
        command.CommandText = createUserTableCmd;
        _ = command.ExecuteNonQuery();

        // Create BingeEntrys table
        var createBingesTableCmd = @"
                CREATE TABLE IF NOT EXISTS BingeEntrys (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Username TEXT NOT NULL,
                    DateTime TEXT NOT NULL,
                    Duration TEXT NOT NULL
                );";

        command.CommandText = createBingesTableCmd;
        _ = command.ExecuteNonQuery();
    }
}