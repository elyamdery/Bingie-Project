using Microsoft.Data.Sqlite;

namespace Bingie.Services;

public class SqliteConnectionFactory
{
    public static string DatabasePath => Path.Combine(FileSystem.AppDataDirectory, "BingieDB.db");

    public SqliteConnection CreateConnection()
    {
        return new SqliteConnection($"Data Source={DatabasePath}");
    }
}