using System.Diagnostics;
using Bingie.Models;
using Bingie.Data;
using Microsoft.EntityFrameworkCore;

namespace Bingie.Views;

public partial class DayStatisticsPage : ContentPage
{
    private readonly AppDBContext _context;
    private readonly DateTime _selectedDate;

    public DayStatisticsPage(DateTime date, AppDBContext context)
    {
        InitializeComponent();
        _selectedDate = date;
        _context = context ?? throw new ArgumentNullException(nameof(context));
        DisplayStatisticsAsync();
    }

    private async void DisplayStatisticsAsync()
    {
        try
        {
            Debug.WriteLine($"DayStatisticsPage: Displaying stats for {_selectedDate}");
            SelectedDateLabel.Text = _selectedDate.ToString("MMMM dd, yyyy");
            var bingeCount = await GetBingeCountForDateAsync(_selectedDate);
            BingeCountLabel.Text = $"You binged {bingeCount} times on this day.";
        }
        catch (Exception ex)
        {
            // Log the exception
            Console.WriteLine($"Error in DisplayStatistics: {ex.Message}");
            BingeCountLabel.Text = "Unable to retrieve binge count.";
        }
    }    private async Task<int> GetBingeCountForDateAsync(DateTime date)
    {
        try
        {
            Debug.WriteLine($"DayStatisticsPage: Retrieving binge count for {date}");
            return await _context.BingeEntries.CountAsync(r => r.Date.Date == date.Date);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Exception in GetBingeCountForDateAsync: {ex.Message}");
            throw;
        }
    }

    private void OnBackButtonClicked(object sender, EventArgs e)
    {
        _ = Navigation.PopAsync();
    }
}