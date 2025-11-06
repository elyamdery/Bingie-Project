using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Bingie.Models;
using Microsoft.Data.Sqlite;

namespace Bingie.Services;

/// <summary>
/// SQLite-backed repository for the friends leaderboard feature.
/// </summary>
public sealed class FriendsLeaderboardRepository : IFriendsLeaderboardRepository
{
    private readonly SqliteConnectionFactory _connectionFactory;

    public FriendsLeaderboardRepository(SqliteConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
    }

    public async Task<FriendCircle?> GetCircleByInviteCodeAsync(string inviteCode)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(inviteCode);
        await using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        await using var command = connection.CreateCommand();
        command.CommandText = @"SELECT Id, Name, InviteCode, CreatedUtc FROM FriendCircles WHERE InviteCode = @code";
        _ = command.Parameters.AddWithValue("@code", inviteCode.Trim().ToUpperInvariant());

        await using var reader = await command.ExecuteReaderAsync();
        return await reader.ReadAsync() ? MapCircle(reader) : null;
    }

    public async Task<FriendCircle?> GetCircleByIdAsync(int circleId)
    {
        await using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        await using var command = connection.CreateCommand();
        command.CommandText = @"SELECT Id, Name, InviteCode, CreatedUtc FROM FriendCircles WHERE Id = @id";
        _ = command.Parameters.AddWithValue("@id", circleId);

        await using var reader = await command.ExecuteReaderAsync();
        return await reader.ReadAsync() ? MapCircle(reader) : null;
    }

    public async Task<int> AddCircleAsync(FriendCircle circle)
    {
        ArgumentNullException.ThrowIfNull(circle);
        await using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        await using var command = connection.CreateCommand();
        command.CommandText = @"
            INSERT INTO FriendCircles (Name, InviteCode, CreatedUtc)
            VALUES (@name, @inviteCode, @createdUtc);
            SELECT last_insert_rowid();";

        _ = command.Parameters.AddWithValue("@name", circle.Name.Trim());
        _ = command.Parameters.AddWithValue("@inviteCode", circle.InviteCode.Trim().ToUpperInvariant());
        _ = command.Parameters.AddWithValue("@createdUtc", circle.CreatedUtc.ToString("O", CultureInfo.InvariantCulture));

        var result = await command.ExecuteScalarAsync();
        return Convert.ToInt32(result, CultureInfo.InvariantCulture);
    }

    public async Task UpdateCircleAsync(FriendCircle circle)
    {
        ArgumentNullException.ThrowIfNull(circle);
        await using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        await using var command = connection.CreateCommand();
        command.CommandText = @"
            UPDATE FriendCircles
            SET Name = @name, InviteCode = @inviteCode
            WHERE Id = @id;";

        _ = command.Parameters.AddWithValue("@name", circle.Name.Trim());
        _ = command.Parameters.AddWithValue("@inviteCode", circle.InviteCode.Trim().ToUpperInvariant());
        _ = command.Parameters.AddWithValue("@id", circle.Id);

        await command.ExecuteNonQueryAsync();
    }

    public async Task<CircleMembership?> GetMembershipAsync(string username)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(username);
        await using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        await using var command = connection.CreateCommand();
        command.CommandText = @"
            SELECT Id, CircleId, Username, Nickname, ShareXp, ShareStreak, ShareCopingCount, Muted, JoinedUtc, LastActiveUtc
            FROM CircleMemberships
            WHERE Username = @username;";
        _ = command.Parameters.AddWithValue("@username", username.Trim());

        await using var reader = await command.ExecuteReaderAsync();
        return await reader.ReadAsync() ? MapMembership(reader) : null;
    }

    public async Task<CircleMembership?> GetMembershipByIdAsync(int membershipId)
    {
        await using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        await using var command = connection.CreateCommand();
        command.CommandText = @"
            SELECT Id, CircleId, Username, Nickname, ShareXp, ShareStreak, ShareCopingCount, Muted, JoinedUtc, LastActiveUtc
            FROM CircleMemberships
            WHERE Id = @id;";
        _ = command.Parameters.AddWithValue("@id", membershipId);

        await using var reader = await command.ExecuteReaderAsync();
        return await reader.ReadAsync() ? MapMembership(reader) : null;
    }

    public async Task<IReadOnlyList<CircleMembership>> GetMembershipsForCircleAsync(int circleId)
    {
        List<CircleMembership> memberships = new();
        await using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        await using var command = connection.CreateCommand();
        command.CommandText = @"
            SELECT Id, CircleId, Username, Nickname, ShareXp, ShareStreak, ShareCopingCount, Muted, JoinedUtc, LastActiveUtc
            FROM CircleMemberships
            WHERE CircleId = @circleId
            ORDER BY JoinedUtc;";
        _ = command.Parameters.AddWithValue("@circleId", circleId);

        await using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            memberships.Add(MapMembership(reader));
        }

        return memberships;
    }

    public async Task<int> AddMembershipAsync(CircleMembership membership)
    {
        ArgumentNullException.ThrowIfNull(membership);
        await using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        await using var command = connection.CreateCommand();
        command.CommandText = @"
            INSERT INTO CircleMemberships (CircleId, Username, Nickname, ShareXp, ShareStreak, ShareCopingCount, Muted, JoinedUtc, LastActiveUtc)
            VALUES (@circleId, @username, @nickname, @shareXp, @shareStreak, @shareCoping, @muted, @joinedUtc, @lastActiveUtc);
            SELECT last_insert_rowid();";

        AddMembershipParameters(command, membership);

        var result = await command.ExecuteScalarAsync();
        return Convert.ToInt32(result, CultureInfo.InvariantCulture);
    }

    public async Task UpdateMembershipAsync(CircleMembership membership)
    {
        ArgumentNullException.ThrowIfNull(membership);
        await using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        await using var command = connection.CreateCommand();
        command.CommandText = @"
            UPDATE CircleMemberships
            SET CircleId = @circleId,
                Username = @username,
                Nickname = @nickname,
                ShareXp = @shareXp,
                ShareStreak = @shareStreak,
                ShareCopingCount = @shareCoping,
                Muted = @muted,
                JoinedUtc = @joinedUtc,
                LastActiveUtc = @lastActiveUtc
            WHERE Id = @id;";

        AddMembershipParameters(command, membership);
        _ = command.Parameters.AddWithValue("@id", membership.Id);

        await command.ExecuteNonQueryAsync();
    }

    public async Task RemoveMembershipAsync(int membershipId)
    {
        await using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        await using var command = connection.CreateCommand();
        command.CommandText = @"DELETE FROM CircleMemberships WHERE Id = @id;";
        _ = command.Parameters.AddWithValue("@id", membershipId);

        await command.ExecuteNonQueryAsync();
    }

    public async Task<IReadOnlyList<CircleWeeklySnapshot>> GetSnapshotsForWeekAsync(int circleId, DateTime weekStartUtc)
    {
        List<CircleWeeklySnapshot> snapshots = new();
        await using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        await using var command = connection.CreateCommand();
        command.CommandText = @"
            SELECT Id, CircleId, Username, WeekStartUtc, SharedXp, SharedStreakDays, SharedCopingCount, CreatedUtc, RefreshedUtc
            FROM CircleWeeklySnapshots
            WHERE CircleId = @circleId AND WeekStartUtc = @weekStart;";
        _ = command.Parameters.AddWithValue("@circleId", circleId);
        _ = command.Parameters.AddWithValue("@weekStart", weekStartUtc.ToString("O", CultureInfo.InvariantCulture));

        await using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            snapshots.Add(MapSnapshot(reader));
        }

        return snapshots;
    }

    public async Task UpsertWeeklySnapshotAsync(CircleWeeklySnapshot snapshot)
    {
        ArgumentNullException.ThrowIfNull(snapshot);
        await using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        await using var command = connection.CreateCommand();
        command.CommandText = @"
            INSERT INTO CircleWeeklySnapshots (CircleId, Username, WeekStartUtc, SharedXp, SharedStreakDays, SharedCopingCount, CreatedUtc, RefreshedUtc)
            VALUES (@circleId, @username, @weekStartUtc, @sharedXp, @sharedStreak, @sharedCoping, @createdUtc, @refreshedUtc)
            ON CONFLICT(CircleId, Username, WeekStartUtc)
            DO UPDATE SET
                SharedXp = excluded.SharedXp,
                SharedStreakDays = excluded.SharedStreakDays,
                SharedCopingCount = excluded.SharedCopingCount,
                RefreshedUtc = excluded.RefreshedUtc;";

        _ = command.Parameters.AddWithValue("@circleId", snapshot.CircleId);
        _ = command.Parameters.AddWithValue("@username", snapshot.Username.Trim());
        _ = command.Parameters.AddWithValue("@weekStartUtc", snapshot.WeekStartUtc.ToString("O", CultureInfo.InvariantCulture));
        _ = command.Parameters.AddWithValue("@sharedXp", snapshot.SharedXp);
        _ = command.Parameters.AddWithValue("@sharedStreak", snapshot.SharedStreakDays);
        _ = command.Parameters.AddWithValue("@sharedCoping", snapshot.SharedCopingCount);
        _ = command.Parameters.AddWithValue("@createdUtc", snapshot.CreatedUtc.ToString("O", CultureInfo.InvariantCulture));
        _ = command.Parameters.AddWithValue("@refreshedUtc", snapshot.RefreshedUtc?.ToString("O", CultureInfo.InvariantCulture) ?? (object)DBNull.Value);

        await command.ExecuteNonQueryAsync();
    }

    public async Task<IReadOnlyList<SupportToken>> GetSupportTokensForWeekAsync(int circleId, DateTime weekStartUtc)
    {
        List<SupportToken> tokens = new();
        await using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        await using var command = connection.CreateCommand();
        command.CommandText = @"
            SELECT Id, CircleId, FromUsername, ToUsername, TemplateCode, Message, WeekStartUtc, CreatedUtc
            FROM CircleSupportTokens
            WHERE CircleId = @circleId AND WeekStartUtc = @weekStart
            ORDER BY CreatedUtc;";
        _ = command.Parameters.AddWithValue("@circleId", circleId);
        _ = command.Parameters.AddWithValue("@weekStart", weekStartUtc.ToString("O", CultureInfo.InvariantCulture));

        await using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            tokens.Add(MapToken(reader));
        }

        return tokens;
    }

    public async Task<int> AddSupportTokenAsync(SupportToken token)
    {
        ArgumentNullException.ThrowIfNull(token);
        await using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        await using var command = connection.CreateCommand();
        command.CommandText = @"
            INSERT INTO CircleSupportTokens (CircleId, FromUsername, ToUsername, TemplateCode, Message, WeekStartUtc, CreatedUtc)
            VALUES (@circleId, @from, @to, @template, @message, @weekStart, @createdUtc);
            SELECT last_insert_rowid();";

        _ = command.Parameters.AddWithValue("@circleId", token.CircleId);
        _ = command.Parameters.AddWithValue("@from", token.FromUsername.Trim());
        _ = command.Parameters.AddWithValue("@to", token.ToUsername.Trim());
        _ = command.Parameters.AddWithValue("@template", token.TemplateCode.Trim());
        _ = command.Parameters.AddWithValue("@message", token.Message);
        _ = command.Parameters.AddWithValue("@weekStart", token.WeekStartUtc.ToString("O", CultureInfo.InvariantCulture));
        _ = command.Parameters.AddWithValue("@createdUtc", token.CreatedUtc.ToString("O", CultureInfo.InvariantCulture));

        var result = await command.ExecuteScalarAsync();
        return Convert.ToInt32(result, CultureInfo.InvariantCulture);
    }

    private static void AddMembershipParameters(SqliteCommand command, CircleMembership membership)
    {
        command.Parameters.Clear();
        _ = command.Parameters.AddWithValue("@circleId", membership.CircleId);
        _ = command.Parameters.AddWithValue("@username", membership.Username.Trim());
        _ = command.Parameters.AddWithValue("@nickname", membership.Nickname.Trim());
        _ = command.Parameters.AddWithValue("@shareXp", membership.ShareXp ? 1 : 0);
        _ = command.Parameters.AddWithValue("@shareStreak", membership.ShareStreak ? 1 : 0);
        _ = command.Parameters.AddWithValue("@shareCoping", membership.ShareCopingCount ? 1 : 0);
        _ = command.Parameters.AddWithValue("@muted", membership.Muted ? 1 : 0);
        _ = command.Parameters.AddWithValue("@joinedUtc", membership.JoinedUtc.ToString("O", CultureInfo.InvariantCulture));
        _ = command.Parameters.AddWithValue("@lastActiveUtc", membership.LastActiveUtc?.ToString("O", CultureInfo.InvariantCulture) ?? (object)DBNull.Value);
    }

    private static FriendCircle MapCircle(SqliteDataReader reader)
    {
        return new FriendCircle
        {
            Id = reader.GetInt32(0),
            Name = reader.GetString(1),
            InviteCode = reader.GetString(2),
            CreatedUtc = DateTime.Parse(reader.GetString(3), CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind)
        };
    }

    private static CircleMembership MapMembership(SqliteDataReader reader)
    {
        return new CircleMembership
        {
            Id = reader.GetInt32(0),
            CircleId = reader.GetInt32(1),
            Username = reader.GetString(2),
            Nickname = reader.GetString(3),
            ShareXp = reader.GetInt32(4) != 0,
            ShareStreak = reader.GetInt32(5) != 0,
            ShareCopingCount = reader.GetInt32(6) != 0,
            Muted = reader.GetInt32(7) != 0,
            JoinedUtc = DateTime.Parse(reader.GetString(8), CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind),
            LastActiveUtc = reader.IsDBNull(9)
                ? null
                : DateTime.Parse(reader.GetString(9), CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind)
        };
    }

    private static CircleWeeklySnapshot MapSnapshot(SqliteDataReader reader)
    {
        return new CircleWeeklySnapshot
        {
            Id = reader.GetInt32(0),
            CircleId = reader.GetInt32(1),
            Username = reader.GetString(2),
            WeekStartUtc = DateTime.Parse(reader.GetString(3), CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind),
            SharedXp = reader.GetInt32(4),
            SharedStreakDays = reader.GetInt32(5),
            SharedCopingCount = reader.GetInt32(6),
            CreatedUtc = DateTime.Parse(reader.GetString(7), CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind),
            RefreshedUtc = reader.IsDBNull(8)
                ? null
                : DateTime.Parse(reader.GetString(8), CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind)
        };
    }

    private static SupportToken MapToken(SqliteDataReader reader)
    {
        return new SupportToken
        {
            Id = reader.GetInt32(0),
            CircleId = reader.GetInt32(1),
            FromUsername = reader.GetString(2),
            ToUsername = reader.GetString(3),
            TemplateCode = reader.GetString(4),
            Message = reader.GetString(5),
            WeekStartUtc = DateTime.Parse(reader.GetString(6), CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind),
            CreatedUtc = DateTime.Parse(reader.GetString(7), CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind)
        };
    }
}
