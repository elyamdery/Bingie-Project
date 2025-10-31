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

        MainPage = new NavigationPage(loginPage)
        {
            BarBackgroundColor = Color.FromArgb("#1F3A93"),
            BarTextColor = Colors.White
        };
    }
}
