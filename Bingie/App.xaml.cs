using Bingie.Loggin;
using Bingie.Services; // Add this using directive
using Bingie.Views.Auth;
using Serilog;

namespace Bingie
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // Configure logging
            LoggingConfiguration.ConfigureLogging();
            Log.Information("Application Started");

            // Initialize the main page with AuthService
            DatabaseService databaseService = new(new SqliteConnectionFactory());
            AuthService authService = new(databaseService);
            MainPage = new NavigationPage(new LoginPage(authService));
        }
    }
}