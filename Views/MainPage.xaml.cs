using Bingie.Models;
using Bingie.Data;
using Serilog;
using Microsoft.EntityFrameworkCore;

namespace Bingie.Views;

public partial class MainPage : ContentPage
{
    private readonly AppDBContext? _context;
    private int _bingeCount;    // Default constructor
    public MainPage()
    {
        InitializeComponent();
        // Use the application's root service provider to get the shared AppDBContext
        _context = IPlatformApplication.Current?.Services?.GetService(typeof(AppDBContext)) as AppDBContext;
        _bingeCount = 0;
        Log.Information("MainPage initialized.");
        StartClock();
        UpdateBingeCountLabel();
    }

    public MainPage(AppDBContext context)
    {
        InitializeComponent();
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _bingeCount = 0;
        Log.Information("MainPage initialized.");
        StartClock();
        UpdateBingeCountLabel();
    }
    private void StartClock()
    {
        Dispatcher.StartTimer(TimeSpan.FromSeconds(1), () =>
        {
            // Log the time update
            Log.Information("Current time: {Time}", DateTime.UtcNow.AddHours(3).ToString("HH:mm:ss"));

            CurrentTimeLabel.Text = DateTime.UtcNow.AddHours(3).ToString("HH:mm:ss"); // Adjust to Israel Time (UTC+3)
            return true;
        });
    }

    private async void OnBingeButtonClicked(object sender, EventArgs e)
    {
        Log.Information("Binge button clicked at {Time}", DateTime.UtcNow.ToString("HH:mm:ss"));

        BoxView dot = new()
        {
            Color = Colors.Red,
            WidthRequest = 15,
            HeightRequest = 15,
            Margin = new Thickness(2, 0, 2, 0)
        };
        if (DotsContainer.Children.Count == 0 ||
            (DotsContainer.Children[DotsContainer.Children.Count - 1] as StackLayout)?.Children.Count >= 10)
            DotsContainer.Children.Add(new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Margin = new Thickness(0, 5)
            });

        var currentRow = DotsContainer.Children[DotsContainer.Children.Count - 1] as StackLayout;
        currentRow?.Children.Add(dot);

        _bingeCount++;
        UpdateBingeCountLabel();

        // --- Add binge entry to DB ---
        if (_context != null)
        {
            var username = Preferences.Get("CurrentUsername", "DefaultUser");
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null)
            {
                var mainPage = Application.Current?.MainPage;
                if (mainPage != null)
                {
                    await mainPage.DisplayAlert("Error", "No user found. Please log in.", "OK");
                }
                return;
            }

            var binge = new BingeEntry
            {
                Activity = "Quick Binge",
                Date = DateTime.Today, // Use DateTime.Today to match HistoryPage week logic
                UserId = user.Id,
                Description = "Added from main page",
                IntensityRating = 5,
                Mood = "Neutral",
                Duration = TimeSpan.FromMinutes(30)
            };

            _context.BingeEntries.Add(binge);
            await _context.SaveChangesAsync();
            _context.ChangeTracker.Clear(); // Ensure context is up-to-date for all consumers
            Log.Information("BingeEntry saved for user {User} on {Date}", user.Username, binge.Date);

            // Optionally, force refresh of HistoryPage if it's in the navigation stack
            var nav = Application.Current?.MainPage as NavigationPage;
            if (nav != null)
            {
                var historyPage = nav.Navigation.NavigationStack.OfType<Bingie.Views.HistoryPage>().FirstOrDefault();
                if (historyPage != null)
                {
                    // Call OnAppearing to refresh data
                    historyPage.GetType().GetMethod("OnAppearing", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public)?.Invoke(historyPage, null);
                }
            }
        }
    }

    private void UpdateBingeCountLabel()
    {
        BingeCountLabel.Text = $"Binge Count: {_bingeCount}";
    }
}
