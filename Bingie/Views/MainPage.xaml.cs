using Serilog;  // Importing Serilog for logging

namespace Bingie.Views
{
    public partial class MainPage : ContentPage
    {
        [Obsolete]
        public MainPage()
        {
            InitializeComponent();

            // Initialize logging in the MainPage as well
            Log.Information("MainPage initialized.");  // <-- Log when the MainPage is initialized

            StartClock();
        }

        [Obsolete]
        private void StartClock()
        {
            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                // Log the time update
                Log.Information("Current time: {Time}", DateTime.UtcNow.AddHours(3).ToString("HH:mm:ss"));

                CurrentTimeLabel.Text = DateTime.UtcNow.AddHours(3).ToString("HH:mm:ss"); // Adjust to Israel Time (UTC+3)
                return true;
            });
        }

        private void OnBingeButtonClicked(object sender, EventArgs e)
        {
            // Log when the binge button is clicked
            Log.Information("Binge button clicked at {Time}", DateTime.UtcNow.ToString("HH:mm:ss"));

            BoxView dot = new()
            {
                Color = Colors.Red,
                WidthRequest = 15, // Make the dot bigger
                HeightRequest = 15, // Make the dot bigger
                Margin = new Thickness(2, 0, 2, 0)
            };

            // Create a new horizontal row every 10 dots
            if (DotsContainer.Children.Count == 0 || (DotsContainer.Children[DotsContainer.Children.Count - 1] as StackLayout).Children.Count >= 10)
            {
                DotsContainer.Children.Add(new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    Margin = new Thickness(0, 5)
                });
            }

            StackLayout? currentRow = DotsContainer.Children[DotsContainer.Children.Count - 1] as StackLayout;
            currentRow.Children.Add(dot);
        }
    }
}
