using System.Diagnostics;
using Bingie.Models;
using Bingie.Services;
using Bingie.Views;
using Bingie.Views.Auth;

namespace Bingie;

public partial class AppShell : Shell
{
    private readonly IDataStore<BingeEntry> _dataStore;
    private readonly string _username;

    public AppShell(IDataStore<BingeEntry> dataStore, string username)
    {
        InitializeComponent();
        _dataStore = dataStore ?? throw new ArgumentNullException(nameof(dataStore));
        _username = username ?? throw new ArgumentNullException(nameof(username));

        // Register routes
        Routing.RegisterRoute("login", typeof(LoginPage));
        Routing.RegisterRoute("history", typeof(HistoryPage));
        Routing.RegisterRoute("daystatistics", typeof(DayStatisticsPage));
        Routing.RegisterRoute("register", typeof(RegistrationPage));
        Routing.RegisterRoute("statistics", typeof(StatisticsPage));
        Routing.RegisterRoute("bingeRecords", typeof(BingeRecordPage));

        // Set the ContentTemplate for the History tab
        var historyTab = new ShellContent
        {
            Title = "History",
            Icon = "history.png",
            Route = "history",
            ContentTemplate = new DataTemplate(() => CreateHistoryPage())
        };

        Items.Add(new TabBar
        {
            Items =
            {
                new ShellContent
                {
                    Title = "Home",
                    Icon = "home.png",
                    Route = "home",
                    ContentTemplate = new DataTemplate(() => new MainPage(_dataStore, _username))
                },
                historyTab,
                new ShellContent
                {
                    Title = "Explore",
                    Icon = "explore.png",
                    Route = "explore",
                    ContentTemplate = new DataTemplate(() => new ExplorePage(_dataStore, _username))
                }
            }
        });

        // Handle navigation events
        Navigated += OnNavigated;
    }

    private HistoryPage CreateHistoryPage()
    {
        return new HistoryPage(_dataStore, _username);
    }

    private void OnNavigated(object sender, ShellNavigatedEventArgs e)
    {
        Debug.WriteLine($"Navigated to: {e.Current.Location}");
    }
}