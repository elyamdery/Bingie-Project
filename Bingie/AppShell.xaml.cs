using System.Diagnostics;
using Bingie.Config;
using Bingie.Messaging;
using Bingie.Models;
using Bingie.Services;
using Bingie.Views;
using Bingie.Views.Auth;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Maui.Controls;

namespace Bingie;

public partial class AppShell : Shell
{
    private readonly IDataStore<BingeEntry> _dataStore;
    private readonly CalendarService _calendarService;
    private readonly AvatarFeedbackService _avatarFeedbackService;
    private readonly StoryGuideService _storyGuideService;
    private readonly PointsSystemService _pointsSystemService;
    private readonly FriendsLeaderboardService _friendsLeaderboardService;
    private readonly TriggerRadarService _triggerRadarService;
    private ShellContent? _friendsShellContent;
    private readonly IAuthService _authService;
    private readonly string _username;
    private readonly IMessenger _messenger = WeakReferenceMessenger.Default;
    private bool _messengerRegistered;

    public AppShell(
        IDataStore<BingeEntry> dataStore,
        CalendarService calendarService,
        AvatarFeedbackService avatarFeedbackService,
        StoryGuideService storyGuideService,
        PointsSystemService pointsSystemService,
        FriendsLeaderboardService friendsLeaderboardService,
        TriggerRadarService triggerRadarService,
        IAuthService authService,
        string username)
    {
        InitializeComponent();
        _dataStore = dataStore ?? throw new ArgumentNullException(nameof(dataStore));
        _calendarService = calendarService ?? throw new ArgumentNullException(nameof(calendarService));
        _avatarFeedbackService = avatarFeedbackService ?? throw new ArgumentNullException(nameof(avatarFeedbackService));
        _storyGuideService = storyGuideService ?? throw new ArgumentNullException(nameof(storyGuideService));
        _pointsSystemService = pointsSystemService ?? throw new ArgumentNullException(nameof(pointsSystemService));
        _friendsLeaderboardService = friendsLeaderboardService ?? throw new ArgumentNullException(nameof(friendsLeaderboardService));
        _triggerRadarService = triggerRadarService ?? throw new ArgumentNullException(nameof(triggerRadarService));
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

        TabBar tabBar = new();
        tabBar.Items.Add(new ShellContent
        {
            Title = "Home",
            Route = "home",
            ContentTemplate = new DataTemplate(() =>
                new MainPage(_dataStore, _calendarService, _avatarFeedbackService, _storyGuideService, _pointsSystemService, _username, _authService))
        });
        tabBar.Items.Add(historyTab);
        tabBar.Items.Add(new ShellContent
        {
            Title = "Explore",
            Route = "explore",
            ContentTemplate = new DataTemplate(() =>
                new ExplorePage(_storyGuideService, _pointsSystemService, _username))
        });

        _friendsShellContent = new ShellContent
        {
            Title = "Friends",
            Route = "friends",
            IsVisible = FeatureFlags.LeaderboardEnabled,
            ContentTemplate = new DataTemplate(() =>
                new FriendsLeaderboardPage(_friendsLeaderboardService, _username))
        };
        tabBar.Items.Add(_friendsShellContent);

        Items.Add(tabBar);
        _messenger.Register<FriendsLeaderboardVisibilityChangedMessage>(this, (_, message) =>
        {
            if (_friendsShellContent != null)
            {
                _friendsShellContent.IsVisible = message.Value;
            }
        });
        _messengerRegistered = true;

        Navigated += OnNavigated;
    }

    protected override void OnHandlerChanged()
    {
        base.OnHandlerChanged();
        if (Handler == null && _messengerRegistered)
        {
            _messenger.Unregister<FriendsLeaderboardVisibilityChangedMessage>(this);
            _messengerRegistered = false;
        }
    }

    private HistoryPage CreateHistoryPage()
    {
        return new HistoryPage(_dataStore, _calendarService, _triggerRadarService, _username);
    }

    private void OnNavigated(object? sender, ShellNavigatedEventArgs e)
    {
        Debug.WriteLine($"Navigated to: {e.Current.Location}");
    }
}
