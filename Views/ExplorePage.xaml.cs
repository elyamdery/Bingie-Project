namespace Bingie.Views;

public partial class ExplorePage : ContentPage
{
    private readonly Random random;
    private List<string> quotes = new();

    public ExplorePage()
    {
        InitializeComponent();
        random = new Random();
        LoadQuotes();
        ShowRandomQuote();
    }

    private void LoadQuotes()
    {
        quotes =
        [
            "Quote 1",
            "Quote 2",
            "Quote 3"
            // Add more quotes here
        ];
    }

    private void ShowRandomQuote()
    {
        if (quotes != null && quotes.Count > 0)
        {
            var index = random.Next(quotes.Count);
            RandomQuoteLabel.Text = quotes[index];
        }
    }
}