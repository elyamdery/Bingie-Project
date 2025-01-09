using Bingie.Views;
using Bingie.Models;
using Bingie.Views.Auth;
using Serilog;
using Bingie.Logging;
using Microsoft.Extensions.Logging;
using Bingie.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Bingie
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            // Configure the app and fonts
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

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
                .WriteTo.File(Path.Combine(FileSystem.AppDataDirectory, "logs", "bingie-log.txt"), rollingInterval: RollingInterval.Day)
                .CreateLogger();

            Log.Information("Logging initialized successfully.");

            // Ensure logs are flushed on application exit
            AppDomain.CurrentDomain.ProcessExit += (s, e) => Log.CloseAndFlush();
        }

        private static void RegisterServices(MauiAppBuilder builder)
        {
            // Register SqliteConnectionFactory
            builder.Services.AddSingleton<SqliteConnectionFactory>();

            // Register DatabaseService using SqliteConnectionFactory
            builder.Services.AddSingleton<IDataStore<BingeEntry>, DatabaseService>();

            // Register DatabaseInitializer
            builder.Services.AddSingleton<DatabaseInitializer>();

            // Register pages
            builder.Services.AddTransient<BingeRecordsPage>();
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<RegistrationPage>();
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<HistoryPage>();
            builder.Services.AddTransient<DayStatisticsPage>();
            builder.Services.AddTransient<ExplorePage>();
            builder.Services.AddTransient<StatisticsPage>();
        }
    }
}