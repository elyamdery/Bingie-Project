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
                        Duration TEXT NOT NULL,
                        WasUrgeResisted INTEGER DEFAULT 0,
                        Points INTEGER DEFAULT 0,
                        Notes TEXT DEFAULT ''
                    );";

            command.CommandText = createBingesTableCmd;
            _ = command.ExecuteNonQuery();

            // Check if we need to add new columns to the existing table
            try {
                // Check if WasUrgeResisted column exists
                command.CommandText = "PRAGMA table_info(BingeEntrys)";
                using var reader = command.ExecuteReader();
                bool hasWasUrgeResisted = false;
                bool hasPoints = false;
                bool hasNotes = false;

                while (reader.Read())
                {
                    string columnName = reader.GetString(1);
                    if (columnName == "WasUrgeResisted") hasWasUrgeResisted = true;
                    if (columnName == "Points") hasPoints = true;
                    if (columnName == "Notes") hasNotes = true;
                }

                // Add missing columns if needed
                if (!hasWasUrgeResisted)
                {
                    command.CommandText = "ALTER TABLE BingeEntrys ADD COLUMN WasUrgeResisted INTEGER DEFAULT 0";
                    command.ExecuteNonQuery();
                    Debug.WriteLine("Added WasUrgeResisted column to BingeEntrys table");
                }

                if (!hasPoints)
                {
                    command.CommandText = "ALTER TABLE BingeEntrys ADD COLUMN Points INTEGER DEFAULT 0";
                    command.ExecuteNonQuery();
                    Debug.WriteLine("Added Points column to BingeEntrys table");
                }

                if (!hasNotes)
                {
                    command.CommandText = "ALTER TABLE BingeEntrys ADD COLUMN Notes TEXT DEFAULT ''";
                    command.ExecuteNonQuery();
                    Debug.WriteLine("Added Notes column to BingeEntrys table");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error checking or adding columns: {ex.Message}");
            }

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