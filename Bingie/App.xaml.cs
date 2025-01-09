using Microsoft.Maui.Controls;
using Bingie.Views.Auth;
using Bingie.Services; // Add this using directive
using Bingie.Logging;
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
            var databaseService = new DatabaseService(new SqliteConnectionFactory());
            var authService = new AuthService(databaseService);
            MainPage = new NavigationPage(new LoginPage(authService));
        }
    }
}