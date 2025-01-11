namespace Bingie.Services
{
    public class DatabaseInitializer
    {
        private readonly SqliteConnectionFactory _connectionFactory;

        public DatabaseInitializer(SqliteConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public void InitializeDatabase()
        {
            using Microsoft.Data.Sqlite.SqliteConnection connection = _connectionFactory.CreateConnection();
            connection.Open();

            // Create Users table
            string createUserTableCmd = @"
                CREATE TABLE IF NOT EXISTS Users (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Username TEXT NOT NULL,
                    Password TEXT NOT NULL
                );";

            using Microsoft.Data.Sqlite.SqliteCommand command = connection.CreateCommand();
            command.CommandText = createUserTableCmd;
            _ = command.ExecuteNonQuery();

            // Create Binges table
            string createBingesTableCmd = @"
                CREATE TABLE IF NOT EXISTS Binges (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Username TEXT NOT NULL,
                    DateTime TEXT NOT NULL,
                    Duration TEXT NOT NULL
                );";

            command.CommandText = createBingesTableCmd;
            _ = command.ExecuteNonQuery();
        }
    }
}