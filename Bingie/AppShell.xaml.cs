using System.Diagnostics;
using Bingie.Models;
using Bingie.Services;
using Bingie.Views;
using Bingie.Views.Auth;

namespace Bingie;

public partial class AppShell : Shell
{
    private readonly IDataStore<BingeEntry> _dataStore;
    private readonly CalendarService _calendarService;
    private readonly PointsSystemService _pointsSystemService;
    private readonly string _username;

    public AppShell(IDataStore<BingeEntry> dataStore, CalendarService calendarService, PointsSystemService pointsSystemService, string username)
    {
        InitializeComponent();
        _dataStore = dataStore ?? throw new ArgumentNullException(nameof(dataStore));
        _calendarService = calendarService ?? throw new ArgumentNullException(nameof(calendarService));
        _pointsSystemService = pointsSystemService ?? throw new ArgumentNullException(nameof(pointsSystemService));
        _username = username ?? throw new ArgumentNullException(nameof(username));

        Items.Clear();

        // Register routes
        Routing.RegisterRoute("login", typeof(LoginPage));
        Routing.RegisterRoute("history", typeof(HistoryPage));
        Routing.RegisterRoute("daystatistics", typeof(DayStatisticsPage));
        Routing.RegisterRoute("register", typeof(RegistrationPage));

        // Set the ContentTemplate for the History tab
        var historyTab = new ShellContent
        {
            Title = "History",
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
                    Route = "home",
                    ContentTemplate = new DataTemplate(() => new MainPage(_dataStore, _calendarService, _pointsSystemService, _username))
                },
                historyTab,
                new ShellContent
                {
                    Title = "Explore",
                    Route = "explore",
                    ContentTemplate = new DataTemplate(() => new ExplorePage(_pointsSystemService, _username))
                }
            }
        });

        // Handle navigation events
        Navigated += OnNavigated;
    }

    private HistoryPage CreateHistoryPage()
    {
        return new HistoryPage(_dataStore, _calendarService, _username);
    }

    private void OnNavigated(object? sender, ShellNavigatedEventArgs e)
    {
        Debug.WriteLine($"Navigated to: {e.Current.Location}");
    }
}
