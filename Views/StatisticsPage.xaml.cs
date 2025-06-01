using Bingie.Models;
using Bingie.Data;
using Microsoft.EntityFrameworkCore;

namespace Bingie.Views;

public partial class StatisticsPage : ContentPage
{
    private readonly AppDBContext _context;

    public StatisticsPage(AppDBContext context)
    {
        InitializeComponent();
        _context = context ?? throw new ArgumentNullException(nameof(context));

        // Load and display the binge count for the user
        LoadBingeCountAsync();

        // Start the page animation (fade in)
        StartPageAnimation();
    }    private async void LoadBingeCountAsync()
    {
        try
        {
            // TODO: Get current user from authentication service
            // For now, we'll count all binge entries
            var bingeCount = await _context.BingeEntries.CountAsync();
            BingeCountLabel.Text = $"Total Binge Count: {bingeCount}";
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to load binge count: {ex.Message}", "OK");
        }
    }

    private async void StartPageAnimation()
    {
        // Fade in animation
        Opacity = 0;
        _ = await this.FadeTo(1, 500); // Fades in over 0.5 seconds
    }
}