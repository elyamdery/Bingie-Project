using System.Diagnostics;
using Bingie.Models;
using Bingie.Views;
using Bingie.Views.Auth;

namespace Bingie;

public partial class AppShell : Shell
{
    private readonly User? _currentUser;

    public AppShell()
    {
        InitializeComponent();
        RegisterRoutes();
        Navigated += OnNavigated;
    }

    public AppShell(User currentUser) : this()
    {
        _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
    }

    private void RegisterRoutes()
    {
        Debug.WriteLine("[AppShell] Registering routes...");
        // Register routes for navigation
        Routing.RegisterRoute("login", typeof(LoginPage));
        Routing.RegisterRoute("registration", typeof(RegistrationPage));
        Routing.RegisterRoute("history", typeof(HistoryPage));
        Routing.RegisterRoute("daystatistics", typeof(DayStatisticsPage));
        Routing.RegisterRoute("statistics", typeof(StatisticsPage));
        Routing.RegisterRoute("bingerecord", typeof(BingeRecordPage));
        Routing.RegisterRoute("explore", typeof(ExplorePage));
    }    public User? GetCurrentUser() => _currentUser;

    private void OnNavigated(object? sender, ShellNavigatedEventArgs e)
    {
        Debug.WriteLine($"Navigated to: {e.Current.Location}");
    }
}