namespace Bingie.Views
{
    public partial class DayStatisticsPage : ContentPage
    {
        private readonly DateTime selectedDate;

        public DayStatisticsPage(DateTime date)
        {
            InitializeComponent();
            selectedDate = date;
            DisplayStatistics();
        }

        private void DisplayStatistics()
        {
            try
            {
                SelectedDateLabel.Text = selectedDate.ToString("MMMM dd, yyyy");
                int bingeCount = GetBingeCountForDate(selectedDate);
                BingeCountLabel.Text = $"You binged {bingeCount} times on this day.";
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error in DisplayStatistics: {ex.Message}");
                BingeCountLabel.Text = "Unable to retrieve binge count.";
            }
        }

        private int GetBingeCountForDate(DateTime date)
        {
            // TODO: Implement actual logic to retrieve binge count from your data storage
            // For now, we'll return a random number between 0 and 5
            return new Random().Next(0, 6);
        }

        private void OnBackButtonClicked(object sender, EventArgs e)
        {
            _ = Navigation.PopAsync();
        }
    }
}