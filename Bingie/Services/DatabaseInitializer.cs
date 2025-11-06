using Microsoft.Data.Sqlite;

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

        EnsureUsersTable(connection);
        EnsureBingeEntriesTable(connection);
        EnsureAvatarFeedbackTables(connection);
        EnsureStoryGuideTables(connection);
        EnsurePointsSystemTables(connection);
        EnsureFriendsLeaderboardTables(connection);
    }

    private static void EnsureUsersTable(SqliteConnection connection)
    {
        using var command = connection.CreateCommand();
        command.CommandText = @"
                CREATE TABLE IF NOT EXISTS Users (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Username TEXT NOT NULL UNIQUE,
                    Password TEXT NOT NULL,
                    RememberToken TEXT
                );";
        _ = command.ExecuteNonQuery();

        TryEnsureRememberTokenColumn(connection);
    }

    private static void TryEnsureRememberTokenColumn(SqliteConnection connection)
    {
        using var ensureCommand = connection.CreateCommand();
        ensureCommand.CommandText = "ALTER TABLE Users ADD COLUMN RememberToken TEXT;";

        try
        {
            _ = ensureCommand.ExecuteNonQuery();
        }
        catch (SqliteException ex) when (ex.SqliteErrorCode == 1)
        {
            // Column already exists; ignore.
        }
    }

    private static void EnsureBingeEntriesTable(SqliteConnection connection)
    {
        using var command = connection.CreateCommand();
        command.CommandText = @"
                CREATE TABLE IF NOT EXISTS BingeEntries (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Username TEXT NOT NULL,
                    Date TEXT NOT NULL,
                    Duration TEXT NOT NULL
                );";
        _ = command.ExecuteNonQuery();
    }

    private static void EnsureAvatarFeedbackTables(SqliteConnection connection)
    {
        using var createCommand = connection.CreateCommand();
        createCommand.CommandText = @"
                CREATE TABLE IF NOT EXISTS AvatarFeedbackSettings (
                    Username TEXT PRIMARY KEY,
                    HideAvatar INTEGER NOT NULL DEFAULT 0,
                    WeeklyGlowThreshold INTEGER NOT NULL,
                    WeeklyConcernThreshold INTEGER NOT NULL,
                    MonthlyGlowThreshold INTEGER NOT NULL,
                    MonthlyConcernThreshold INTEGER NOT NULL,
                    LastUpdatedUtc TEXT NOT NULL
                );";
        _ = createCommand.ExecuteNonQuery();

        createCommand.CommandText = @"
                CREATE TABLE IF NOT EXISTS AvatarStateHistory (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Username TEXT NOT NULL,
                    PeriodStartUtc TEXT NOT NULL,
                    PeriodType TEXT NOT NULL,
                    Score REAL NOT NULL,
                    EnergyState TEXT NOT NULL,
                    DeltaFromPrevious REAL NOT NULL,
                    SupportiveCopy TEXT NOT NULL,
                    CreatedUtc TEXT NOT NULL,
                    UNIQUE(Username, PeriodType, PeriodStartUtc)
                );";
        _ = createCommand.ExecuteNonQuery();

        createCommand.CommandText = @"
                CREATE INDEX IF NOT EXISTS IX_AvatarStateHistory_UserPeriod
                ON AvatarStateHistory (Username, PeriodType, PeriodStartUtc);";
        _ = createCommand.ExecuteNonQuery();
    }

    private static void EnsureStoryGuideTables(SqliteConnection connection)
    {
        using var command = connection.CreateCommand();
        command.CommandText = @"
                CREATE TABLE IF NOT EXISTS StoryTriggerSelections (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Username TEXT NOT NULL,
                    EntryDateUtc TEXT NOT NULL,
                    TriggerCode TEXT NOT NULL,
                    CustomTrigger TEXT,
                    CreatedUtc TEXT NOT NULL,
                    UNIQUE(Username, EntryDateUtc)
                );";
        _ = command.ExecuteNonQuery();

        command.CommandText = @"
                CREATE INDEX IF NOT EXISTS IX_StoryTriggerSelections_UserDate
                ON StoryTriggerSelections (Username, EntryDateUtc);";
        _ = command.ExecuteNonQuery();

        command.CommandText = @"
                CREATE TABLE IF NOT EXISTS StoryExperimentProgress (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Username TEXT NOT NULL,
                    ExperimentCode TEXT NOT NULL,
                    Status INTEGER NOT NULL,
                    LastSuggestedUtc TEXT,
                    PlannedUtc TEXT,
                    CompletedUtc TEXT,
                    XpGranted INTEGER NOT NULL DEFAULT 0,
                    UNIQUE(Username, ExperimentCode)
                );";
        _ = command.ExecuteNonQuery();

        command.CommandText = @"
                CREATE INDEX IF NOT EXISTS IX_StoryExperimentProgress_User
                ON StoryExperimentProgress (Username);";
        _ = command.ExecuteNonQuery();
    }

    private static void EnsurePointsSystemTables(SqliteConnection connection)
    {
        using var command = connection.CreateCommand();
        command.CommandText = @"
                CREATE TABLE IF NOT EXISTS PointActions (
                    ActionCode TEXT PRIMARY KEY,
                    Title TEXT NOT NULL,
                    Description TEXT NOT NULL,
                    XpValue INTEGER NOT NULL,
                    DailyLimit INTEGER NOT NULL DEFAULT 1,
                    QuestEligible INTEGER NOT NULL DEFAULT 1
                );";
        _ = command.ExecuteNonQuery();

        command.CommandText = @"
                CREATE TABLE IF NOT EXISTS PointDailyQuests (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Username TEXT NOT NULL,
                    ActionCode TEXT NOT NULL,
                    QuestDateUtc TEXT NOT NULL,
                    CreatedUtc TEXT NOT NULL,
                    CompletedUtc TEXT,
                    XpAwarded INTEGER NOT NULL DEFAULT 0,
                    UNIQUE(Username, ActionCode, QuestDateUtc)
                );";
        _ = command.ExecuteNonQuery();

        command.CommandText = @"
                CREATE INDEX IF NOT EXISTS IX_PointDailyQuests_UserDate
                ON PointDailyQuests (Username, QuestDateUtc);";
        _ = command.ExecuteNonQuery();

        command.CommandText = @"
                CREATE TABLE IF NOT EXISTS PointXpTransactions (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Username TEXT NOT NULL,
                    ActionCode TEXT NOT NULL,
                    Amount INTEGER NOT NULL,
                    CreatedUtc TEXT NOT NULL
                );";
        _ = command.ExecuteNonQuery();

        command.CommandText = @"
                CREATE INDEX IF NOT EXISTS IX_PointXpTransactions_UserDate
                ON PointXpTransactions (Username, CreatedUtc);";
        _ = command.ExecuteNonQuery();

        command.CommandText = @"
                CREATE TABLE IF NOT EXISTS PointSettings (
                    Username TEXT PRIMARY KEY,
                    RewardsPaused INTEGER NOT NULL DEFAULT 0,
                    EquippedCosmeticCode TEXT
                );";
        _ = command.ExecuteNonQuery();

        command.CommandText = @"
                CREATE TABLE IF NOT EXISTS PointCosmetics (
                    CosmeticCode TEXT PRIMARY KEY,
                    Name TEXT NOT NULL,
                    Description TEXT NOT NULL,
                    RequiredXp INTEGER NOT NULL
                );";
        _ = command.ExecuteNonQuery();

        command.CommandText = @"
                CREATE TABLE IF NOT EXISTS PointCosmeticUnlocks (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Username TEXT NOT NULL,
                    CosmeticCode TEXT NOT NULL,
                    UnlockedUtc TEXT NOT NULL,
                    Equipped INTEGER NOT NULL DEFAULT 0,
                    UNIQUE(Username, CosmeticCode)
                );";
        _ = command.ExecuteNonQuery();

        SeedDefaultPointActions(connection);
        SeedDefaultCosmetics(connection);
    }

    private static void SeedDefaultPointActions(SqliteConnection connection)
    {
        using var insert = connection.CreateCommand();
        insert.CommandText = @"
                INSERT OR IGNORE INTO PointActions (ActionCode, Title, Description, XpValue, DailyLimit, QuestEligible)
                VALUES
                    ('log_entry', 'Log a check-in', 'Record a binge or urge with compassion.', 30, 3, 1),
                    ('drink_water', 'Hydration pause', 'Drink a glass of water mindfully.', 20, 2, 1),
                    ('reflect_feelings', 'Name your feeling', 'Write a sentence about what you felt.', 25, 2, 1),
                    ('breathing_break', 'Breathing break', 'Try a 2-minute breathing exercise.', 20, 2, 1),
                    ('kind_note', 'Leave a kind note', 'Send yourself or someone else a kind message.', 15, 2, 1),
                    ('stretch_reset', 'Stretch reset', 'Do a gentle stretch series.', 15, 2, 1),
                    ('avatar_care', 'Avatar care sync', 'Check in with your avatar energy summary.', 10, 1, 0);
            ";
        _ = insert.ExecuteNonQuery();
    }

    private static void SeedDefaultCosmetics(SqliteConnection connection)
    {
        using var insert = connection.CreateCommand();
        insert.CommandText = @"
                INSERT OR IGNORE INTO PointCosmetics (CosmeticCode, Name, Description, RequiredXp)
                VALUES
                    ('glow_ring', 'Glow Ring', 'A luminous ring around your avatar.', 100),
                    ('aurora_trail', 'Aurora Trail', 'A soft aurora trail that follows your avatar.', 250),
                    ('starlight_canvas', 'Starlight Canvas', 'Starry background for nightly reflections.', 500);
            ";
        _ = insert.ExecuteNonQuery();
    }

    private static void EnsureFriendsLeaderboardTables(SqliteConnection connection)
    {
        using var command = connection.CreateCommand();
        command.CommandText = @"
                CREATE TABLE IF NOT EXISTS FriendCircles (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL,
                    InviteCode TEXT NOT NULL UNIQUE,
                    CreatedUtc TEXT NOT NULL
                );";
        _ = command.ExecuteNonQuery();

        command.CommandText = @"
                CREATE TABLE IF NOT EXISTS CircleMemberships (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    CircleId INTEGER NOT NULL,
                    Username TEXT NOT NULL UNIQUE,
                    Nickname TEXT NOT NULL,
                    ShareXp INTEGER NOT NULL DEFAULT 1,
                    ShareStreak INTEGER NOT NULL DEFAULT 1,
                    ShareCopingCount INTEGER NOT NULL DEFAULT 1,
                    Muted INTEGER NOT NULL DEFAULT 0,
                    JoinedUtc TEXT NOT NULL,
                    LastActiveUtc TEXT,
                    FOREIGN KEY (CircleId) REFERENCES FriendCircles(Id) ON DELETE CASCADE
                );";
        _ = command.ExecuteNonQuery();

        command.CommandText = @"
                CREATE INDEX IF NOT EXISTS IX_CircleMemberships_Circle
                ON CircleMemberships (CircleId);";
        _ = command.ExecuteNonQuery();

        command.CommandText = @"
                CREATE TABLE IF NOT EXISTS CircleWeeklySnapshots (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    CircleId INTEGER NOT NULL,
                    Username TEXT NOT NULL,
                    WeekStartUtc TEXT NOT NULL,
                    SharedXp INTEGER NOT NULL,
                    SharedStreakDays INTEGER NOT NULL,
                    SharedCopingCount INTEGER NOT NULL,
                    CreatedUtc TEXT NOT NULL,
                    RefreshedUtc TEXT,
                    UNIQUE (CircleId, Username, WeekStartUtc),
                    FOREIGN KEY (CircleId) REFERENCES FriendCircles(Id) ON DELETE CASCADE
                );";
        _ = command.ExecuteNonQuery();

        command.CommandText = @"
                CREATE INDEX IF NOT EXISTS IX_CircleWeeklySnapshots_Week
                ON CircleWeeklySnapshots (CircleId, WeekStartUtc);";
        _ = command.ExecuteNonQuery();

        command.CommandText = @"
                CREATE TABLE IF NOT EXISTS CircleSupportTokens (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    CircleId INTEGER NOT NULL,
                    FromUsername TEXT NOT NULL,
                    ToUsername TEXT NOT NULL,
                    TemplateCode TEXT NOT NULL,
                    Message TEXT NOT NULL,
                    WeekStartUtc TEXT NOT NULL,
                    CreatedUtc TEXT NOT NULL,
                    FOREIGN KEY (CircleId) REFERENCES FriendCircles(Id) ON DELETE CASCADE
                );";
        _ = command.ExecuteNonQuery();

        command.CommandText = @"
                CREATE INDEX IF NOT EXISTS IX_CircleSupportTokens_CircleWeek
                ON CircleSupportTokens (CircleId, WeekStartUtc);";
        _ = command.ExecuteNonQuery();
    }
}
