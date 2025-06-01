using Bingie.Data;
using Bingie.Services;
using Bingie.Views;
using Bingie.Views.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

namespace Bingie;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        // Configure the app and fonts
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts => { fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular"); });

        // Configure Serilog for logging
        ConfigureLogging();

        // Register services for dependency injection
        RegisterServices(builder);

        // Configure logging
        builder.Logging.AddSerilog(dispose: true);

        // Build the app
        var app = builder.Build();

        try
        {
            // Initialize the database after the app is built
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDBContext>();
            context.Database.EnsureCreated();
            Log.Information("Database initialized successfully");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error initializing database");
        }

        return app;
    }

    private static void ConfigureLogging()
    {
        var logDirectory = Path.Combine(FileSystem.AppDataDirectory, "logs");

        // Ensure the logs directory exists
        if (!Directory.Exists(logDirectory))
            Directory.CreateDirectory(logDirectory);

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.File(Path.Combine(logDirectory, "bingie-log.txt"),
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: 7)
            .CreateLogger();

        Log.Information("Logging initialized successfully.");

        // Ensure logs are flushed on application exit
        AppDomain.CurrentDomain.ProcessExit += (s, e) => Log.CloseAndFlush();
    }    private static void RegisterServices(MauiAppBuilder builder)
    {
        // Configure Entity Framework with SQLite
        var connectionString = $"Data Source={Path.Combine(FileSystem.AppDataDirectory, "bingie.db")}";
        
        builder.Services.AddDbContext<AppDBContext>(options =>
            options.UseSqlite(connectionString));

        // Register business services
        builder.Services.AddScoped<IAuthService, AuthService>();
        builder.Services.AddScoped<IBingeService, BingeService>();

        // Register pages for dependency injection
        builder.Services.AddTransient<BingeRecordPage>();
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<RegistrationPage>();
        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<HistoryPage>();
        builder.Services.AddTransient<DayStatisticsPage>();
        builder.Services.AddTransient<ExplorePage>();
        builder.Services.AddTransient<StatisticsPage>();
    }
}