using Bingie.Loggin;
using Bingie.Views.Auth;
using Serilog;

namespace Bingie;

public partial class App : Application
{
    public App(LoginPage loginPage)
    {
        InitializeComponent();

        LoggingConfiguration.ConfigureLogging();
        Log.Information("Application Started");

        MainPage = loginPage;
    }
}
