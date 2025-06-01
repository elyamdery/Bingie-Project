using Bingie.Models;
using Bingie.Data;
using Microsoft.EntityFrameworkCore;

namespace Bingie.Views;

public partial class BingeRecordPage : ContentPage
{    private readonly AppDBContext _context;

    public BingeRecordPage(AppDBContext context)
    {
        InitializeComponent();
        _context = context ?? throw new ArgumentNullException(nameof(context));
        LoadBingeRecordsAsync();
    }

    private async void LoadBingeRecordsAsync()
    {
        try
        {
            // TODO: Get current user from authentication service
            // For now, we'll load all entries for today
            var todayRecords = await _context.BingeEntries
                .Include(b => b.User)
                .Where(b => b.Date.Date == DateTime.Today)
                .ToListAsync();
            
            BingeRecordsCollectionView.ItemsSource = todayRecords;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to load records: {ex.Message}", "OK");        }
    }

    private async void OnAddBingeRecordClicked(object sender, EventArgs e)
    {
        try
        {
            // TODO: Get current user ID from authentication service
            // For now, ensure we have a valid user or create a default one
            var currentUser = await _context.Users.FirstOrDefaultAsync();
            if (currentUser == null)
            {
                // Create a default user for testing purposes
                currentUser = new User
                {
                    Username = "DefaultUser",
                    Email = "default@example.com",
                    Password = "hashedpassword", // In real app, this would be properly hashed
                };
                _context.Users.Add(currentUser);
                await _context.SaveChangesAsync();
            }

            var newRecord = new BingeEntry
            {
                Activity = "Default Activity", // Required property
                Date = DateTime.Now,
                UserId = currentUser.Id,
                Description = "New binge entry",
                IntensityRating = 5,
                Mood = "Neutral",
                Duration = TimeSpan.FromMinutes(30) // Add a default duration
            };

            _context.BingeEntries.Add(newRecord);
            await _context.SaveChangesAsync();            
            LoadBingeRecordsAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to add record: {ex.Message}", "OK");
        }
    }
}