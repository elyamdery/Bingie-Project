using Bingie.Models;
using Serilog;

namespace Bingie.Services;

public class DatabaseService : IDataStore<BingeEntry>, IDataStore<User>
{
    private readonly SqliteConnectionFactory _connectionFactory;

    public DatabaseService(SqliteConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    // Explicit implementation for BingeEntry
    async Task<bool> IDataStore<BingeEntry>.AddItemAsync(BingeEntry item)
    {
        try
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync();

            var insertCmd = @"
                    INSERT INTO BingeEntrys (Username, DateTime, Duration, WasUrgeResisted, Points, Notes)
                    VALUES (@username, @dateTime, @duration, @wasUrgeResisted, @points, @notes);";

            using var command = connection.CreateCommand();
            command.CommandText = insertCmd;
            _ = command.Parameters.AddWithValue("@username", item.Username);
            _ = command.Parameters.AddWithValue("@dateTime", item.Date.ToString("yyyy-MM-dd HH:mm:ss"));
            _ = command.Parameters.AddWithValue("@duration", item.Duration.ToString());
            _ = command.Parameters.AddWithValue("@wasUrgeResisted", item.WasUrgeResisted ? 1 : 0);
            _ = command.Parameters.AddWithValue("@points", item.Points);
            _ = command.Parameters.AddWithValue("@notes", item.Notes ?? string.Empty);

            _ = await command.ExecuteNonQueryAsync();
            Log.Information("Added new binge entry for user {Username}", item.Username);
            return true;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error adding binge entry for user {Username}", item.Username);
            return false;
        }
    }

    // Explicit implementation for BingeEntry
    async Task<IEnumerable<BingeEntry>> IDataStore<BingeEntry>.GetItemsAsync()
    {
        try
        {
            List<BingeEntry> entries = new();
            using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync();

            var selectCmd = "SELECT * FROM BingeEntrys;";
            using var command = connection.CreateCommand();
            command.CommandText = selectCmd;

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var entry = new BingeEntry
                {
                    Id = reader.GetInt32(0),
                    Username = reader.GetString(1),
                    Date = DateTime.Parse(reader.GetString(2)),
                    Duration = TimeSpan.Parse(reader.GetString(3))
                };

                // Handle new columns which might not exist in older database versions
                try
                {
                    entry.WasUrgeResisted = reader.GetInt32(4) == 1;
                    entry.Points = reader.GetInt32(5);
                    entry.Notes = reader.IsDBNull(6) ? string.Empty : reader.GetString(6);
                }
                catch (IndexOutOfRangeException)
                {
                    // Column doesn't exist in this version of the database, use defaults
                    Log.Warning("Some columns are missing in the database. Using default values.");
                }

                entries.Add(entry);
            }

            return entries;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error retrieving binge entries");
            return new List<BingeEntry>();
        }
    }

    // Explicit implementation for BingeEntry
    async Task<BingeEntry> IDataStore<BingeEntry>.GetItemAsync(string id)
    {
        try
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync();

            var selectCmd = "SELECT * FROM BingeEntrys WHERE Id = @id;";
            using var command = connection.CreateCommand();
            command.CommandText = selectCmd;
            _ = command.Parameters.AddWithValue("@id", id);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                var entry = new BingeEntry
                {
                    Id = reader.GetInt32(0),
                    Username = reader.GetString(1),
                    Date = DateTime.Parse(reader.GetString(2)),
                    Duration = TimeSpan.Parse(reader.GetString(3))
                };

                // Handle new columns which might not exist in older database versions
                try
                {
                    entry.WasUrgeResisted = reader.GetInt32(4) == 1;
                    entry.Points = reader.GetInt32(5);
                    entry.Notes = reader.IsDBNull(6) ? string.Empty : reader.GetString(6);
                }
                catch (IndexOutOfRangeException)
                {
                    // Column doesn't exist in this version of the database, use defaults
                    Log.Warning("Some columns are missing in the database. Using default values.");
                }

                return entry;
            }

            return null;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error retrieving binge entry with ID {Id}", id);
            return null;
        }
    }

    // Explicit implementation for BingeEntry
    async Task<bool> IDataStore<BingeEntry>.UpdateItemAsync(BingeEntry item)
    {
        try
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync();

            var updateCmd = @"
                    UPDATE BingeEntrys
                    SET Username = @username, DateTime = @dateTime, Duration = @duration,
                        WasUrgeResisted = @wasUrgeResisted, Points = @points, Notes = @notes
                    WHERE Id = @id;";

            using var command = connection.CreateCommand();
            command.CommandText = updateCmd;
            _ = command.Parameters.AddWithValue("@username", item.Username);
            _ = command.Parameters.AddWithValue("@dateTime", item.Date.ToString("yyyy-MM-dd HH:mm:ss"));
            _ = command.Parameters.AddWithValue("@duration", item.Duration.ToString());
            _ = command.Parameters.AddWithValue("@wasUrgeResisted", item.WasUrgeResisted ? 1 : 0);
            _ = command.Parameters.AddWithValue("@points", item.Points);
            _ = command.Parameters.AddWithValue("@notes", item.Notes ?? string.Empty);
            _ = command.Parameters.AddWithValue("@id", item.Id);

            _ = await command.ExecuteNonQueryAsync();
            Log.Information("Updated binge entry with ID {Id}", item.Id);
            return true;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error updating binge entry with ID {Id}", item.Id);
            return false;
        }
    }

    // Explicit implementation for BingeEntry
    async Task<bool> IDataStore<BingeEntry>.DeleteItemAsync(string id)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        var deleteCmd = "DELETE FROM BingeEntrys WHERE Id = @id;";
        using var command = connection.CreateCommand();
        command.CommandText = deleteCmd;
        _ = command.Parameters.AddWithValue("@id", id);

        _ = await command.ExecuteNonQueryAsync();
        return true;
    }

    // Additional method to get binge entries by username and date range
    public async Task<IEnumerable<BingeEntry>> GetBingeEntriesByDateRangeAsync(string username, DateTime startDate, DateTime endDate)
    {
        try
        {
            List<BingeEntry> entries = new();
            using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync();

            var selectCmd = @"SELECT * FROM BingeEntrys
                            WHERE Username = @username
                            AND DateTime BETWEEN @startDate AND @endDate
                            ORDER BY DateTime DESC;";

            using var command = connection.CreateCommand();
            command.CommandText = selectCmd;
            _ = command.Parameters.AddWithValue("@username", username);
            _ = command.Parameters.AddWithValue("@startDate", startDate.ToString("yyyy-MM-dd HH:mm:ss"));
            _ = command.Parameters.AddWithValue("@endDate", endDate.ToString("yyyy-MM-dd HH:mm:ss"));

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var entry = new BingeEntry
                {
                    Id = reader.GetInt32(0),
                    Username = reader.GetString(1),
                    Date = DateTime.Parse(reader.GetString(2)),
                    Duration = TimeSpan.Parse(reader.GetString(3))
                };

                // Handle new columns which might not exist in older database versions
                try
                {
                    entry.WasUrgeResisted = reader.GetInt32(4) == 1;
                    entry.Points = reader.GetInt32(5);
                    entry.Notes = reader.IsDBNull(6) ? string.Empty : reader.GetString(6);
                }
                catch (IndexOutOfRangeException)
                {
                    // Column doesn't exist in this version of the database, use defaults
                }

                entries.Add(entry);
            }

            return entries;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error getting binge entries by date range for user {Username}", username);
            return new List<BingeEntry>();
        }
    }

    // Method to get today's binge entries for a user
    public async Task<IEnumerable<BingeEntry>> GetTodaysBingeEntriesAsync(string username)
    {
        var today = DateTime.Today;
        var tomorrow = today.AddDays(1);
        return await GetBingeEntriesByDateRangeAsync(username, today, tomorrow);
    }

    // Explicit implementation for User
    async Task<bool> IDataStore<User>.AddItemAsync(User item)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        var insertCmd = @"
                INSERT INTO Users (Username, Password)
                VALUES (@username, @password);";

        using var command = connection.CreateCommand();
        command.CommandText = insertCmd;
        _ = command.Parameters.AddWithValue("@username", item.Username);
        _ = command.Parameters.AddWithValue("@password", item.Password);

        _ = await command.ExecuteNonQueryAsync();
        return true;
    }

    // Explicit implementation for User
    async Task<IEnumerable<User>> IDataStore<User>.GetItemsAsync()
    {
        List<User> users = new();
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        var selectCmd = "SELECT * FROM Users;";
        using var command = connection.CreateCommand();
        command.CommandText = selectCmd;

        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
            users.Add(new User
            {
                Id = reader.GetInt32(0),
                Username = reader.GetString(1),
                Password = reader.GetString(2)
            });

        return users;
    }

    // Explicit implementation for User
    async Task<User> IDataStore<User>.GetItemAsync(string id)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        var selectCmd = "SELECT * FROM Users WHERE Id = @id;";
        using var command = connection.CreateCommand();
        command.CommandText = selectCmd;
        _ = command.Parameters.AddWithValue("@id", id);

        using var reader = await command.ExecuteReaderAsync();
        return await reader.ReadAsync()
            ? new User
            {
                Id = reader.GetInt32(0),
                Username = reader.GetString(1),
                Password = reader.GetString(2)
            }
            : null;
    }

    // Explicit implementation for User
    async Task<bool> IDataStore<User>.UpdateItemAsync(User item)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        var updateCmd = @"
                UPDATE Users
                SET Username = @username, Password = @password
                WHERE Id = @id;";

        using var command = connection.CreateCommand();
        command.CommandText = updateCmd;
        _ = command.Parameters.AddWithValue("@username", item.Username);
        _ = command.Parameters.AddWithValue("@password", item.Password);
        _ = command.Parameters.AddWithValue("@id", item.Id);

        _ = await command.ExecuteNonQueryAsync();
        return true;
    }

    // Explicit implementation for User
    async Task<bool> IDataStore<User>.DeleteItemAsync(string id)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        var deleteCmd = "DELETE FROM Users WHERE Id = @id;";
        using var command = connection.CreateCommand();
        command.CommandText = deleteCmd;
        _ = command.Parameters.AddWithValue("@id", id);

        _ = await command.ExecuteNonQueryAsync();
        return true;
    }
}