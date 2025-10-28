using System;
using System.Globalization;
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
                INSERT INTO BingeEntries (Username, Date, Duration)
                VALUES (@username, @date, @duration);";

        using var command = connection.CreateCommand();
        command.CommandText = insertCmd;
        _ = command.Parameters.AddWithValue("@username", item.Username);
        _ = command.Parameters.AddWithValue("@date", item.Date.ToString("O"));
        _ = command.Parameters.AddWithValue("@duration", item.Duration.ToString("c", CultureInfo.InvariantCulture));

        _ = await command.ExecuteNonQueryAsync();
        return true;
    }

    // Explicit implementation for BingeEntry
    async Task<IEnumerable<BingeEntry>> IDataStore<BingeEntry>.GetItemsAsync()
    {
        List<BingeEntry> entries = new();
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        var selectCmd = "SELECT * FROM BingeEntries;";
        using var command = connection.CreateCommand();
        command.CommandText = selectCmd;

        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
            entries.Add(new BingeEntry
            {
                Id = reader.GetInt32(0),
                Username = reader.GetString(1),
                Date = DateTime.Parse(reader.GetString(2), CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind),
                Duration = TimeSpan.Parse(reader.GetString(3), CultureInfo.InvariantCulture)
            });

        return entries;
    }

    // Explicit implementation for BingeEntry
    async Task<BingeEntry?> IDataStore<BingeEntry>.GetItemAsync(string id)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        var selectCmd = "SELECT * FROM BingeEntries WHERE Id = @id;";
        using var command = connection.CreateCommand();
        command.CommandText = selectCmd;
        _ = command.Parameters.AddWithValue("@id", id);

        using var reader = await command.ExecuteReaderAsync();
        return await reader.ReadAsync()
            ? new BingeEntry
            {
                Id = reader.GetInt32(0),
                Username = reader.GetString(1),
                Date = DateTime.Parse(reader.GetString(2), CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind),
                Duration = TimeSpan.Parse(reader.GetString(3), CultureInfo.InvariantCulture)
            }
            : null;
    }

    // Explicit implementation for BingeEntry
    async Task<bool> IDataStore<BingeEntry>.UpdateItemAsync(BingeEntry item)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        var updateCmd = @"
                UPDATE BingeEntries 
                SET Username = @username, Date = @date, Duration = @duration 
                WHERE Id = @id;";

        using var command = connection.CreateCommand();
        command.CommandText = updateCmd;
        _ = command.Parameters.AddWithValue("@username", item.Username);
        _ = command.Parameters.AddWithValue("@date", item.Date.ToString("O"));
        _ = command.Parameters.AddWithValue("@duration", item.Duration.ToString("c", CultureInfo.InvariantCulture));
        _ = command.Parameters.AddWithValue("@id", item.Id);

        _ = await command.ExecuteNonQueryAsync();
        return true;
    }

    // Explicit implementation for BingeEntry
    async Task<bool> IDataStore<BingeEntry>.DeleteItemAsync(string id)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        var deleteCmd = "DELETE FROM BingeEntries WHERE Id = @id;";
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
                INSERT INTO Users (Username, Password, RememberToken)
                VALUES (@username, @password, @rememberToken);";

        using var command = connection.CreateCommand();
        command.CommandText = insertCmd;
        _ = command.Parameters.AddWithValue("@username", item.Username);
        _ = command.Parameters.AddWithValue("@password", item.Password);
        _ = command.Parameters.AddWithValue("@rememberToken", (object?)item.RememberToken ?? DBNull.Value);

        _ = await command.ExecuteNonQueryAsync();
        return true;
    }

    // Explicit implementation for User
    async Task<IEnumerable<User>> IDataStore<User>.GetItemsAsync()
    {
        List<User> users = new();
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        var selectCmd = "SELECT Id, Username, Password, RememberToken FROM Users;";
        using var command = connection.CreateCommand();
        command.CommandText = selectCmd;

        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
            users.Add(new User
            {
                Id = reader.GetInt32(0),
                Username = reader.GetString(1),
                Password = reader.GetString(2),
                RememberToken = reader.IsDBNull(3) ? null : reader.GetString(3)
            });

        return users;
    }

    // Explicit implementation for User
    async Task<User?> IDataStore<User>.GetItemAsync(string id)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        var selectCmd = "SELECT Id, Username, Password, RememberToken FROM Users WHERE Id = @id;";
        using var command = connection.CreateCommand();
        command.CommandText = selectCmd;
        _ = command.Parameters.AddWithValue("@id", id);

        using var reader = await command.ExecuteReaderAsync();
        return await reader.ReadAsync()
            ? new User
            {
                Id = reader.GetInt32(0),
                Username = reader.GetString(1),
                Password = reader.GetString(2),
                RememberToken = reader.IsDBNull(3) ? null : reader.GetString(3)
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
                SET Username = @username, Password = @password, RememberToken = @rememberToken 
                WHERE Id = @id;";

        using var command = connection.CreateCommand();
        command.CommandText = updateCmd;
        _ = command.Parameters.AddWithValue("@username", item.Username);
        _ = command.Parameters.AddWithValue("@password", item.Password);
        _ = command.Parameters.AddWithValue("@rememberToken", (object?)item.RememberToken ?? DBNull.Value);
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
