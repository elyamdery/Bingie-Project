namespace Bingie.Services;

public class DataStore<T> : IDataStore<T> where T : class
{
    private readonly SqliteConnectionFactory _connectionFactory;

    public DataStore(SqliteConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<bool> AddItemAsync(T item)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        var tableName =
            typeof(T).Name == "BingeEntry" ? "BingeEntrys" : typeof(T).Name + "s"; // Correct table name for BingeEntry
        var properties = typeof(T).GetProperties();
        var columns = string.Join(", ", properties.Select(p => p.Name));
        var values = string.Join(", ", properties.Select(p => $"@{p.Name}"));

        var insertCmd = $"INSERT INTO {tableName} ({columns}) VALUES ({values});";

        using var command = connection.CreateCommand();
        command.CommandText = insertCmd;

        foreach (var property in properties)
            command.Parameters.AddWithValue($"@{property.Name}", property.GetValue(item));

        await command.ExecuteNonQueryAsync();
        return true;
    }

    public async Task<T> GetItemAsync(string id)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        var tableName =
            typeof(T).Name == "BingeEntry" ? "BingeEntrys" : typeof(T).Name + "s"; // Correct table name for BingeEntry
        var selectCmd = $"SELECT * FROM {tableName} WHERE Id = @id;";

        using var command = connection.CreateCommand();
        command.CommandText = selectCmd;
        command.Parameters.AddWithValue("@id", id);

        using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            var item = Activator.CreateInstance<T>();
            foreach (var property in typeof(T).GetProperties())
            {
                var value = reader[property.Name];
                property.SetValue(item, value);
            }

            return item;
        }

        return null;
    }

    public async Task<IEnumerable<T>> GetItemsAsync()
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        var tableName =
            typeof(T).Name == "BingeEntry" ? "BingeEntrys" : typeof(T).Name + "s"; // Correct table name for BingeEntry
        var selectCmd = $"SELECT * FROM {tableName};";

        using var command = connection.CreateCommand();
        command.CommandText = selectCmd;

        using var reader = await command.ExecuteReaderAsync();
        var items = new List<T>();
        while (await reader.ReadAsync())
        {
            var item = Activator.CreateInstance<T>();
            foreach (var property in typeof(T).GetProperties())
            {
                var value = reader[property.Name];
                property.SetValue(item, value);
            }

            items.Add(item);
        }

        return items;
    }

    public async Task<bool> UpdateItemAsync(T item)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        var tableName =
            typeof(T).Name == "BingeEntry" ? "BingeEntrys" : typeof(T).Name + "s"; // Correct table name for BingeEntry
        var properties = typeof(T).GetProperties();
        var setClause = string.Join(", ", properties.Select(p => $"{p.Name} = @{p.Name}"));

        var updateCmd = $"UPDATE {tableName} SET {setClause} WHERE Id = @Id;";

        using var command = connection.CreateCommand();
        command.CommandText = updateCmd;

        foreach (var property in properties)
            command.Parameters.AddWithValue($"@{property.Name}", property.GetValue(item));

        await command.ExecuteNonQueryAsync();
        return true;
    }

    public async Task<bool> DeleteItemAsync(string id)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        var tableName =
            typeof(T).Name == "BingeEntry" ? "BingeEntrys" : typeof(T).Name + "s"; // Correct table name for BingeEntry
        var deleteCmd = $"DELETE FROM {tableName} WHERE Id = @id;";

        using var command = connection.CreateCommand();
        command.CommandText = deleteCmd;
        command.Parameters.AddWithValue("@id", id);

        await command.ExecuteNonQueryAsync();
        return true;
    }
}