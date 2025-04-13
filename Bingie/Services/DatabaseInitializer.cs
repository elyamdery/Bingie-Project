using System.Diagnostics;

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
        try
        {
            using var connection = _connectionFactory.CreateConnection();
            connection.Open();

            // Create Users table
            var createUserTableCmd = @"
                    CREATE TABLE IF NOT EXISTS Users (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Username TEXT NOT NULL UNIQUE,
                        Password TEXT NOT NULL
                    );";

            using var command = connection.CreateCommand();
            command.CommandText = createUserTableCmd;
            _ = command.ExecuteNonQuery();

            // Create BingeEntrys table (match the name used in DatabaseService)
            var createBingesTableCmd = @"
                    CREATE TABLE IF NOT EXISTS BingeEntrys (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Username TEXT NOT NULL,
                        DateTime TEXT NOT NULL,
                        Duration TEXT NOT NULL
                    );";

            command.CommandText = createBingesTableCmd;
            _ = command.ExecuteNonQuery();

            // Check if we need to add a default user
            command.CommandText = "SELECT COUNT(*) FROM Users;";
            var count = Convert.ToInt32(command.ExecuteScalar());

            if (count == 0)
            {
                // Add a default user for testing
                command.CommandText = "INSERT INTO Users (Username, Password) VALUES (@username, @password);";
                command.Parameters.AddWithValue("@username", "test");
                command.Parameters.AddWithValue("@password", "test");
                command.ExecuteNonQuery();

                Debug.WriteLine("Added default user: username=test, password=test");
            }

            Debug.WriteLine("Database initialized successfully");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error initializing database: {ex.Message}");
        }
    }
}