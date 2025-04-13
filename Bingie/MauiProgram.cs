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
        // Register SqliteConnectionFactory
        _ = builder.Services.AddSingleton<SqliteConnectionFactory>();

        // Register DatabaseService using SqliteConnectionFactory
        _ = builder.Services.AddSingleton<IDataStore<BingeEntry>, DatabaseService>();

        // Register DatabaseInitializer
        _ = builder.Services.AddSingleton<DatabaseInitializer>();

        // Register pages
        _ = builder.Services.AddTransient<BingeRecordPage>();
        _ = builder.Services.AddTransient<LoginPage>();
        _ = builder.Services.AddTransient<RegistrationPage>();
        _ = builder.Services.AddTransient<MainPage>();
        _ = builder.Services.AddTransient<HistoryPage>();
        _ = builder.Services.AddTransient<DayStatisticsPage>();
        _ = builder.Services.AddTransient<ExplorePage>();
        _ = builder.Services.AddTransient<StatisticsPage>();
    }
}