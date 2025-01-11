using Bingie.Models;

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
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        var insertCmd = @"
                INSERT INTO Binges (Username, DateTime, Duration)
                VALUES (@username, @dateTime, @duration);";

        using var command = connection.CreateCommand();
        command.CommandText = insertCmd;
        _ = command.Parameters.AddWithValue("@username", item.Username);
        _ = command.Parameters.AddWithValue("@dateTime", item.Date.ToString("yyyy-MM-dd HH:mm:ss"));
        _ = command.Parameters.AddWithValue("@duration", item.Duration.ToString());

        _ = await command.ExecuteNonQueryAsync();
        return true;
    }

    // Explicit implementation for BingeEntry
    async Task<IEnumerable<BingeEntry>> IDataStore<BingeEntry>.GetItemsAsync()
    {
        List<BingeEntry> entries = [];
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        var selectCmd = "SELECT * FROM Binges;";
        using var command = connection.CreateCommand();
        command.CommandText = selectCmd;

        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
            entries.Add(new BingeEntry
            {
                Id = reader.GetInt32(0),
                Username = reader.GetString(1),
                Date = DateTime.Parse(reader.GetString(2)),
                Duration = TimeSpan.Parse(reader.GetString(3))
            });

        return entries;
    }

    // Explicit implementation for BingeEntry
    async Task<BingeEntry?> IDataStore<BingeEntry>.GetItemAsync(string id)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        var selectCmd = "SELECT * FROM Binges WHERE Id = @id;";
        using var command = connection.CreateCommand();
        command.CommandText = selectCmd;
        _ = command.Parameters.AddWithValue("@id", id);

        using var reader = await command.ExecuteReaderAsync();
        return await reader.ReadAsync()
            ? new BingeEntry
            {
                Id = reader.GetInt32(0),
                Username = reader.GetString(1),
                Date = DateTime.Parse(reader.GetString(2)),
                Duration = TimeSpan.Parse(reader.GetString(3))
            }
            : null;
    }

    // Explicit implementation for BingeEntry
    async Task<bool> IDataStore<BingeEntry>.UpdateItemAsync(BingeEntry item)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        var updateCmd = @"
                UPDATE Binges 
                SET Username = @username, DateTime = @dateTime, Duration = @duration 
                WHERE Id = @id;";

        using var command = connection.CreateCommand();
        command.CommandText = updateCmd;
        _ = command.Parameters.AddWithValue("@username", item.Username);
        _ = command.Parameters.AddWithValue("@dateTime", item.Date.ToString("yyyy-MM-dd HH:mm:ss"));
        _ = command.Parameters.AddWithValue("@duration", item.Duration.ToString());
        _ = command.Parameters.AddWithValue("@id", item.Id);

        _ = await command.ExecuteNonQueryAsync();
        return true;
    }

    // Explicit implementation for BingeEntry
    async Task<bool> IDataStore<BingeEntry>.DeleteItemAsync(string id)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        var deleteCmd = "DELETE FROM Binges WHERE Id = @id;";
        using var command = connection.CreateCommand();
        command.CommandText = deleteCmd;
        _ = command.Parameters.AddWithValue("@id", id);

        _ = await command.ExecuteNonQueryAsync();
        return true;
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
        List<User> users = [];
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
    async Task<User?> IDataStore<User>.GetItemAsync(string id)
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