using Bingie.Models;
using Bingie.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics; // Add this for Debug.WriteLine

namespace Bingie.Views;

public partial class HistoryPage : ContentPage
{
    private readonly AppDBContext _context;
    private DateTime _currentWeekStart;
    private User? _currentUser;    // Add parameterless constructor for debugging DI issues
    public HistoryPage() 
    {
        Console.WriteLine("[HistoryPage] Parameterless constructor called - DI may have failed!");
        InitializeComponent();
        Title = "History - DI ERROR";
        
        // Create a temporary context to prevent null reference exceptions
        // This is just for debugging - in real app this shouldn't happen
        var connectionString = $"Data Source={Path.Combine(FileSystem.AppDataDirectory, "bingie.db")}";
        var optionsBuilder = new DbContextOptionsBuilder<AppDBContext>();
        optionsBuilder.UseSqlite(connectionString);
        _context = new AppDBContext(optionsBuilder.Options);
        
        _currentWeekStart = GetStartOfWeek(DateTime.Today);
    }

    public HistoryPage(AppDBContext context)
    {
        Console.WriteLine("[HistoryPage] Constructor called."); // Changed to Console.WriteLine
        Debug.WriteLine("[HistoryPage] Constructor called.");
        InitializeComponent();
        _context = context ?? throw new ArgumentNullException(nameof(context));
        
        // Wire up event handlers programmatically
        PreviousWeekButton.Clicked += OnPreviousWeekClicked;
        NextWeekButton.Clicked += OnNextWeekClicked;
        ShowCurrentDayStatsButton.Clicked += OnShowCurrentDayStatsClicked;
        
        _currentWeekStart = GetStartOfWeek(DateTime.Today);
    }    protected override async void OnAppearing()
    {
        base.OnAppearing();
        Console.WriteLine("[HistoryPage] OnAppearing started."); // Changed to Console.WriteLine
        Debug.WriteLine("[HistoryPage] OnAppearing started.");
        
        // Force a visible indication that we're here
        Title = "History - LOADING...";
        
        await LoadCurrentUserAsync();
        Debug.WriteLine($"[HistoryPage] Current user: {(_currentUser?.Username ?? "null")}");
          // Create sample data if none exists (for testing)
        if (_currentUser != null)
        {
            try
            {
                var existingEntries = await _context.BingeEntries.CountAsync(b => b.UserId == _currentUser.Id);
                Debug.WriteLine($"[HistoryPage] Existing entries for user: {existingEntries}");
                if (existingEntries == 0)
                {
                    await CreateSampleDataAsync();
                    Debug.WriteLine("[HistoryPage] Sample data creation attempted.");
                    await DisplayAlert("Welcome", "Sample data has been created to demonstrate the History page functionality.", "OK");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[HistoryPage] Error with sample data: {ex.Message}");
                // Continue without sample data - the calendar will show 0 entries
            }
        }
        
        await LoadWeeklyDataAsync();
        Debug.WriteLine("[HistoryPage] OnAppearing finished.");
    }private async Task LoadCurrentUserAsync()
    {
        try
        {
            Debug.WriteLine("[HistoryPage] LoadCurrentUserAsync started.");
            // Get current user from Shell if available
            if (Shell.Current is AppShell appShell)
            {
                _currentUser = appShell.GetCurrentUser();
                Debug.WriteLine($"[HistoryPage] User from AppShell: {(_currentUser?.Username ?? "null")}");
            }

            // Fallback: get from preferences
            if (_currentUser == null)
            {
                var userId = Preferences.Get("CurrentUserId", -1);
                Debug.WriteLine($"[HistoryPage] User ID from Preferences: {userId}");
                if (userId != -1)
                {
                    _currentUser = await _context.Users.FindAsync(userId);
                    Debug.WriteLine($"[HistoryPage] User from DB (via Preferences): {(_currentUser?.Username ?? "null")}");
                }
            }

            // If still no user, try to find any existing user or create a default one
            if (_currentUser == null)
            {
                Debug.WriteLine("[HistoryPage] No user found yet, trying FirstOrDefaultAsync.");
                _currentUser = await _context.Users.FirstOrDefaultAsync();
                Debug.WriteLine($"[HistoryPage] User from FirstOrDefaultAsync: {(_currentUser?.Username ?? "null")}");
                
                // Create a default user if none exists
                if (_currentUser == null)
                {
                    Debug.WriteLine("[HistoryPage] No user in DB, creating TestUser.");
                    _currentUser = new User
                    {
                        Username = "TestUser",
                        Email = "test@example.com",
                        Password = "testpassword", // Consider hashing in a real app
                        CreatedAt = DateTime.UtcNow
                    };
                    _context.Users.Add(_currentUser);
                    await _context.SaveChangesAsync();
                    Debug.WriteLine("[HistoryPage] TestUser created and saved.");
                }
            }
            Debug.WriteLine($"[HistoryPage] LoadCurrentUserAsync finished. Final user: {(_currentUser?.Username ?? "null")}");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to load user: {ex.Message}", "OK");
        }
    }private async Task LoadWeeklyDataAsync()
    {
        try
        {
            Debug.WriteLine("[HistoryPage] LoadWeeklyDataAsync started.");
            if (_currentUser == null)
            {
                Debug.WriteLine("[HistoryPage] LoadWeeklyDataAsync: _currentUser is null, returning.");
                return;
            }

            UpdateWeekLabel();
            await PopulateCalendarAsync();
            Debug.WriteLine("[HistoryPage] LoadWeeklyDataAsync finished.");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to load weekly data: {ex.Message}", "OK");
        }
    }

    private void UpdateWeekLabel()
    {
        var weekEnd = _currentWeekStart.AddDays(6);
        WeekNumberLabel.Text = $"{_currentWeekStart:MMM dd} - {weekEnd:MMM dd, yyyy}";
        Debug.WriteLine($"[HistoryPage] Week label updated: {WeekNumberLabel.Text}");
    }    private async Task PopulateCalendarAsync()
    {
        try
        {
            Debug.WriteLine("[HistoryPage] PopulateCalendarAsync started.");
            if (_currentUser == null)
            {
                Debug.WriteLine("[HistoryPage] PopulateCalendarAsync: _currentUser is null, displaying alert and returning.");
                await DisplayAlert("Error", "No user logged in", "OK");
                return;
            }

            Debug.WriteLine($"[HistoryPage] Clearing CalendarGrid. Children count before: {CalendarGrid.Children.Count}, RowDefs before: {CalendarGrid.RowDefinitions.Count}");
            CalendarGrid.Children.Clear(); // Make sure this is the correct Grid name from XAML
            CalendarGrid.RowDefinitions.Clear();
            CalendarGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            Debug.WriteLine($"[HistoryPage] CalendarGrid cleared. Children count after: {CalendarGrid.Children.Count}, RowDefs after: {CalendarGrid.RowDefinitions.Count}");


            var weekEnd = _currentWeekStart.AddDays(6);
            Debug.WriteLine($"[HistoryPage] Fetching entries for user {_currentUser.Id} from {_currentWeekStart.Date} to {weekEnd.Date}");
            var weeklyEntries = await _context.BingeEntries
                .Include(b => b.User)
                .Where(b => b.UserId == _currentUser.Id && 
                           b.Date.Date >= _currentWeekStart.Date && 
                           b.Date.Date <= weekEnd.Date)
                .ToListAsync();
            Debug.WriteLine($"[HistoryPage] Found {weeklyEntries.Count} entries for the week.");

            // Add day headers
            string[] dayNames = { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };
            for (int i = 0; i < 7; i++)
            {
                var headerLabel = new Label
                {
                    Text = dayNames[i],
                    HorizontalOptions = LayoutOptions.Center,
                    FontAttributes = FontAttributes.Bold
                };
                Grid.SetRow(headerLabel, 0);
                Grid.SetColumn(headerLabel, i);
                CalendarGrid.Children.Add(headerLabel);
            }
            Debug.WriteLine($"[HistoryPage] Day headers added. CalendarGrid children count: {CalendarGrid.Children.Count}");

            // Add day content
            CalendarGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star }); // Ensure this is flexible
            Debug.WriteLine($"[HistoryPage] Added second row definition for day content. RowDefs count: {CalendarGrid.RowDefinitions.Count}");
            for (int i = 0; i < 7; i++)
            {
                var currentDay = _currentWeekStart.AddDays(i);
                var dayEntries = weeklyEntries.Where(e => e.Date.Date == currentDay.Date).ToList();
                Debug.WriteLine($"[HistoryPage] Processing day {currentDay.DayOfWeek} ({currentDay.Date}): {dayEntries.Count} entries.");

                var dayButton = new Button
                {
                    Text = $"{currentDay.Day}\\n{dayEntries.Count} entries", // Corrected newline for Button text
                    BackgroundColor = dayEntries.Any() ? Colors.LightBlue : Colors.LightGray,
                    VerticalOptions = LayoutOptions.FillAndExpand, // Ensure button fills space
                    HorizontalOptions = LayoutOptions.FillAndExpand // Ensure button fills space
                };

                var day = currentDay; // Capture for closure
                dayButton.Clicked += async (s, e) => await OnDayClickedAsync(day);

                Grid.SetRow(dayButton, 1);
                Grid.SetColumn(dayButton, i);
                CalendarGrid.Children.Add(dayButton);
            }
            Debug.WriteLine($"[HistoryPage] Day content added. CalendarGrid children count: {CalendarGrid.Children.Count}");
            Debug.WriteLine("[HistoryPage] PopulateCalendarAsync finished.");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to populate calendar: {ex.Message}", "OK");
        }
    }

    private async Task OnDayClickedAsync(DateTime selectedDate)
    {        try
        {
            await Shell.Current.GoToAsync($"daystatistics?date={selectedDate:yyyy-MM-dd}");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to navigate to day statistics: {ex.Message}", "OK");
        }
    }    public void OnPreviousWeekClicked(object sender, EventArgs e)
    {
        _currentWeekStart = _currentWeekStart.AddDays(-7);
        _ = LoadWeeklyDataAsync(); // Fire and forget
    }

    public void OnNextWeekClicked(object sender, EventArgs e)
    {
        _currentWeekStart = _currentWeekStart.AddDays(7);
        _ = LoadWeeklyDataAsync(); // Fire and forget
    }

    public void OnShowCurrentDayStatsClicked(object sender, EventArgs e)
    {
        _ = NavigateToCurrentDayStatsAsync(); // Fire and forget
    }

    private async Task NavigateToCurrentDayStatsAsync()
    {
        try
        {
            await Shell.Current.GoToAsync($"//daystatistics?date={DateTime.Today:yyyy-MM-dd}");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to navigate to current day statistics: {ex.Message}", "OK");
        }
    }    private DateTime GetStartOfWeek(DateTime date)
    {
        var diff = (7 + (date.DayOfWeek - DayOfWeek.Sunday)) % 7;
        return date.AddDays(-diff).Date;
    }    private async Task CreateSampleDataAsync()
    {
        try
        {
            Debug.WriteLine("[HistoryPage] CreateSampleDataAsync started.");
            if (_currentUser == null)
            {
                Debug.WriteLine("[HistoryPage] CreateSampleDataAsync: _currentUser is null, returning.");
                return;
            }

            Debug.WriteLine($"[HistoryPage] Creating sample data for user ID: {_currentUser.Id}");

            // Check if database and tables exist
            await CheckDatabaseAsync();

            // Create sample binge entries for the current week
            var sampleEntries = new List<BingeEntry>
            {
                new BingeEntry
                {
                    UserId = _currentUser.Id,
                    Activity = "Social Media",
                    Description = "Scrolled through Instagram for hours",
                    Date = DateTime.Today.AddDays(-2),
                    Duration = TimeSpan.FromHours(2),
                    IntensityRating = 7,
                    Mood = "Anxious",
                    CreatedAt = DateTime.UtcNow
                },
                new BingeEntry
                {
                    UserId = _currentUser.Id,
                    Activity = "Video Games", 
                    Description = "Played games instead of working",
                    Date = DateTime.Today.AddDays(-1),
                    Duration = TimeSpan.FromHours(4),
                    IntensityRating = 8,
                    Mood = "Guilty",
                    CreatedAt = DateTime.UtcNow
                },
                new BingeEntry
                {
                    UserId = _currentUser.Id,
                    Activity = "Netflix",
                    Description = "Binge-watched entire season", 
                    Date = DateTime.Today,
                    Duration = TimeSpan.FromHours(6),
                    IntensityRating = 9,
                    Mood = "Overwhelmed",
                    CreatedAt = DateTime.UtcNow
                }
            };

            Debug.WriteLine($"[HistoryPage] Adding {sampleEntries.Count} entries to context...");
            _context.BingeEntries.AddRange(sampleEntries);
            
            Debug.WriteLine("[HistoryPage] Saving changes to database...");
            await _context.SaveChangesAsync();
            Debug.WriteLine($"[HistoryPage] CreateSampleDataAsync: Successfully added {sampleEntries.Count} sample entries.");
        }catch (Microsoft.EntityFrameworkCore.DbUpdateException dbEx)
        {
            Debug.WriteLine($"[HistoryPage] DbUpdateException: {dbEx.Message}");
            Debug.WriteLine($"[HistoryPage] Inner exception: {dbEx.InnerException?.Message}");
            Debug.WriteLine($"[HistoryPage] Inner-inner exception: {dbEx.InnerException?.InnerException?.Message}");

            // Display the error to the user
            if (dbEx.InnerException != null)
            {
                await DisplayAlert("Database Error", 
                    $"Failed to save sample data: {dbEx.InnerException.Message}", "OK");
            }
            else
            {
                await DisplayAlert("Database Error", 
                    $"Failed to save sample data: {dbEx.Message}", "OK");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"[HistoryPage] General exception: {ex.Message}");
            await DisplayAlert("Error", $"Failed to create sample data: {ex.Message}", "OK");
        }
    }

    private async Task CheckDatabaseAsync()
    {
        try
        {
            Debug.WriteLine("[HistoryPage] Checking if database and tables exist...");
            await _context.Database.EnsureCreatedAsync();
            Debug.WriteLine("[HistoryPage] Database and tables are ready.");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"[HistoryPage] Error ensuring database: {ex.Message}");
            throw;
        }
    }
}
