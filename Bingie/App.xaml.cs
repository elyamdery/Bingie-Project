using Bingie.Loggin;
using Bingie.Models;
using Bingie.Services;
using Bingie.Views.Auth;
using Serilog;

namespace Bingie;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        // Configure logging
        LoggingConfiguration.ConfigureLogging();
        Log.Information("Application Started");

        // Initialize services
        SqliteConnectionFactory connectionFactory = new();

        // Initialize database
        DatabaseInitializer databaseInitializer = new(connectionFactory);
        databaseInitializer.InitializeDatabase();

        DatabaseService databaseService = new(connectionFactory);
        AuthService authService = new(databaseService);

        // Log database path for debugging
        Log.Information("Database path: {Path}", SqliteConnectionFactory.DatabasePath);

        // TEMPORARY: Always bypass authentication for testing
        Log.Information("Bypassing authentication for testing");

        // Create data store and set up main shell
        IDataStore<BingeEntry> dataStore = new DatabaseService(connectionFactory);
        MainPage = new AppShell(dataStore, "test");

        /* Original authentication code
        // Check if user is already authenticated
        if (authService.IsAuthenticated())
        {
            Log.Information("User already authenticated: {Username}", authService.GetCurrentUsername());

            // Create data store and set up main shell
            IDataStore<BingeEntry> dataStore = new DatabaseService(connectionFactory);
            MainPage = new AppShell(dataStore, authService.GetCurrentUsername());
        }
        else
        {
            // User needs to log in
            Log.Information("No authenticated user found, showing login page");
            MainPage = new NavigationPage(new LoginPage(authService));
        }
        */
    }
}