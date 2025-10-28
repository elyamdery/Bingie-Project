using System;
using System.Collections.Generic;
using System.Linq;
using Bingie.Services;
using Microsoft.Maui.Controls.Shapes;

namespace Bingie.Views;

public partial class ExplorePage : ContentPage
{
    private readonly Random _random = new();
    private readonly IReadOnlyList<string> _microChallenges = new List<string>
    {
        "Sip a glass of water before your next craving check-in.",
        "Text a friend one encouraging sentence.",
        "Write down three feelings you notice right now.",
        "Step outside for five breaths of fresh air.",
        "Swap one negative thought for a curious question.",
        "Play your favorite song and stretch for 60 seconds.",
        "Plan a nourishing snack you’re excited about.",
        "List two wins from this week—no matter how small.",
        "Take a mindful bite: notice flavor, texture, and gratitude.",
        "Repeat a mantra: “I deserve care and calm.”"
    };

    public ExplorePage()
    {
        InitializeComponent();
        PopulateChallenges();
        ShowRandomQuote();
    }

    private void PopulateChallenges()
    {
        ChallengesLayout.Children.Clear();

        foreach (var challenge in _microChallenges.OrderBy(_ => _random.Next()).Take(6))
        {
            ChallengesLayout.Children.Add(CreateChallengeCard(challenge));
        }
    }

    private View CreateChallengeCard(string challenge)
    {
        Border card = new()
        {
            BackgroundColor = Color.FromArgb("#FFFFFF"),
            StrokeShape = new RoundRectangle
            {
                CornerRadius = new CornerRadius(18)
            },
            WidthRequest = 150,
            Margin = new Thickness(6),
            Shadow = new Shadow
            {
                Brush = new SolidColorBrush(Color.FromArgb("#1A000000")),
                Radius = 12,
                Offset = new Point(0, 6),
                Opacity = 0.3f
            },
            Content = new VerticalStackLayout
            {
                Padding = new Thickness(14),
                Spacing = 6,
                Children =
                {
                    new Label
                    {
                        Text = challenge,
                        TextColor = Color.FromArgb("#223355"),
                        FontSize = 14
                    }
                }
            }
        };

        card.GestureRecognizers.Add(new TapGestureRecognizer
        {
            Command = new Command(async () =>
            {
                await card.ScaleTo(0.94, 90, Easing.CubicOut);
                await card.ScaleTo(1, 120, Easing.CubicIn);
            })
        });

        return card;
    }

    private void ShowRandomQuote()
    {
        var quote = QuotesService.GetRandomQuote(_random);
        QuoteTextLabel.Text = $"“{quote.Text}”";
        QuoteAuthorLabel.Text = $"- {quote.Author}";
        _ = QuoteCard.ScaleTo(1.02, 110, Easing.CubicOut);
        _ = QuoteCard.ScaleTo(1, 150, Easing.CubicIn);
    }

    private void OnShuffleQuoteClicked(object sender, EventArgs e)
    {
        ShowRandomQuote();
    }
}
