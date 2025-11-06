using Bingie.Models;
using Bingie.Services;
using Bingie.Views;
using Bingie.Views.Auth;
using Serilog;

namespace Bingie;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        // Configure the app and fonts
        _ = builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts => { _ = fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular"); });

        // Configure Serilog for logging
        ConfigureLogging();

        // Register services for dependency injection
        RegisterServices(builder);

        // Build the app
        var app = builder.Build();

        // Initialize the database after the app is built
        var databaseInitializer = app.Services.GetRequiredService<DatabaseInitializer>();
        databaseInitializer.InitializeDatabase();

        return app;
    }

    private static void ConfigureLogging()
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .WriteTo.File(Path.Combine(FileSystem.AppDataDirectory, "logs", "bingie-log.txt"),
                rollingInterval: RollingInterval.Day)
            .CreateLogger();

        Log.Information("Logging initialized successfully.");

        // Ensure logs are flushed on application exit
        AppDomain.CurrentDomain.ProcessExit += (s, e) => Log.CloseAndFlush();
    }

    private static void RegisterServices(MauiAppBuilder builder)
    {
        // Register database primitives
        _ = builder.Services.AddSingleton<SqliteConnectionFactory>();
        _ = builder.Services.AddSingleton<DatabaseService>();
        _ = builder.Services.AddSingleton<IDataStore<BingeEntry>>(sp => sp.GetRequiredService<DatabaseService>());
        _ = builder.Services.AddSingleton<IDataStore<User>>(sp => sp.GetRequiredService<DatabaseService>());
        _ = builder.Services.AddSingleton<IAuthService, AuthService>();
        _ = builder.Services.AddSingleton<CalendarService>();
        _ = builder.Services.AddSingleton<DatabaseInitializer>();
        _ = builder.Services.AddSingleton<IAvatarFeedbackRepository, AvatarFeedbackRepository>();
        _ = builder.Services.AddSingleton<AvatarFeedbackService>();
        _ = builder.Services.AddSingleton<IStoryGuideRepository, StoryGuideRepository>();
        _ = builder.Services.AddSingleton<IStoryGuideAnalytics, NoopStoryGuideAnalytics>();
        _ = builder.Services.AddSingleton<StoryGuideService>();
        _ = builder.Services.AddSingleton<IPointsSystemRepository, PointsSystemRepository>();
        _ = builder.Services.AddSingleton<PointsSystemService>();
        _ = builder.Services.AddSingleton<IFriendsLeaderboardRepository, FriendsLeaderboardRepository>();
        _ = builder.Services.AddSingleton<FriendsLeaderboardService>();

        // Register pages that participate in navigation
        _ = builder.Services.AddTransient<LoginPage>();
        _ = builder.Services.AddTransient<RegistrationPage>();
    }
}
