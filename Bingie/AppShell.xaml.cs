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
    private readonly AvatarFeedbackService _avatarFeedbackService;
    private readonly StoryGuideService _storyGuideService;
    private readonly PointsSystemService _pointsSystemService;
    private readonly IAuthService _authService;
    private readonly string _username;

    public AppShell(
        IDataStore<BingeEntry> dataStore,
        CalendarService calendarService,
        AvatarFeedbackService avatarFeedbackService,
        StoryGuideService storyGuideService,
        PointsSystemService pointsSystemService,
        IAuthService authService,
        string username)
    {
        InitializeComponent();
        _dataStore = dataStore ?? throw new ArgumentNullException(nameof(dataStore));
        _calendarService = calendarService ?? throw new ArgumentNullException(nameof(calendarService));
        _avatarFeedbackService = avatarFeedbackService ?? throw new ArgumentNullException(nameof(avatarFeedbackService));
        _storyGuideService = storyGuideService ?? throw new ArgumentNullException(nameof(storyGuideService));
        _pointsSystemService = pointsSystemService ?? throw new ArgumentNullException(nameof(pointsSystemService));
        _authService = authService ?? throw new ArgumentNullException(nameof(authService));
        _username = username ?? throw new ArgumentNullException(nameof(username));

        Items.Clear();

        Routing.RegisterRoute("login", typeof(LoginPage));
        Routing.RegisterRoute("history", typeof(HistoryPage));
        Routing.RegisterRoute("daystatistics", typeof(DayStatisticsPage));
        Routing.RegisterRoute("register", typeof(RegistrationPage));

        var historyTab = new ShellContent
        {
            Title = "History",
            Route = "history",
            ContentTemplate = new DataTemplate(CreateHistoryPage)
        };

        Items.Add(new TabBar
        {
            Items =
            {
                new ShellContent
                {
                    Title = "Home",
                    Route = "home",
                    ContentTemplate = new DataTemplate(() =>
                        new MainPage(_dataStore, _calendarService, _avatarFeedbackService, _storyGuideService, _pointsSystemService, _username, _authService))
                },
                historyTab,
                new ShellContent
                {
                    Title = "Explore",
                    Route = "explore",
                    ContentTemplate = new DataTemplate(() =>
                        new ExplorePage(_storyGuideService, _pointsSystemService, _username))
                }
            }
        });

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
