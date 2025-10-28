using Bingie.Services;

namespace Bingie.Tests;

public class QuotesServiceTests
{
    [Fact]
    public void GetAllQuotes_ContainsAtLeastOneHundredEntries()
    {
        var allQuotes = QuotesService.GetAllQuotes();

        Assert.True(allQuotes.Count >= 100, $"Expected at least 100 quotes, found {allQuotes.Count}.");
        Assert.All(allQuotes, quote =>
        {
            Assert.False(string.IsNullOrWhiteSpace(quote.Text));
            Assert.False(string.IsNullOrWhiteSpace(quote.Author));
        });
    }

    [Fact]
    public void GetRandomQuote_ReturnsQuoteFromCollection()
    {
        var randomQuote = QuotesService.GetRandomQuote();
        var allQuotes = QuotesService.GetAllQuotes();

        Assert.Contains(randomQuote, allQuotes);
    }
}
